using System.Collections;
using System.Globalization;
using System.Resources;
using Crowdin.Maui.Infrastructure;
using Crowdin.Maui.Models;
using JetBrains.Annotations;

#nullable enable

namespace Crowdin.Maui;

[PublicAPI]
public static class DynamicResourcesLoader
{
    private static CultureInfo? mCustomCulture;
    private static readonly CultureInfo DefaultCulture = CultureInfo.CurrentUICulture;
    
    public static CultureInfo CurrentCulture
    {
        get => mCustomCulture ?? DefaultCulture;
        set => mCustomCulture = value;
    }
    
    public static CrowdinOptions GlobalOptions { get; set; } = new();
    
    public static void LoadStaticStrings(ResourceManager resourceManager, ResourceDictionary dictionary)
    {
        ResourceSet? resourceSet = resourceManager.GetResourceSet(CurrentCulture, true, true);
        
        foreach (DictionaryEntry entry in resourceSet)
        {
            if (entry.Key is string keyString &&
                entry.Value is string valueString)
            {
                dictionary[keyString] = valueString;
            }
        }
    }
    
    public static Task LoadCrowdinStrings(string filename, ResourceDictionary destinationResources)
    {
        var options = (CrowdinOptions) GlobalOptions.Clone();
        options.FileName = filename;
        return LoadCrowdinStrings(options, destinationResources);
    }
    
    /* Flow
     * 
     * 1) Cache disabled, network denied -> return
     * 
     * 2) Cache disabled, network allowed -> get directly from Crowdin -> update resources
     *
     * 3) Cache enabled, not saved yet, network denied -> return
     *
     * 4) Cache enabled, not saved yet, network allowed -> get from Crowdin
     *    Secondary thread: save to cache
     * 
     * 5) Cache enabled, cache found -> use cache
     *    Secondary thread:
     *    1) Check network denied -> return
     *    2) Update manifest
     *    3) Check Cache.UpdatedAt == Manifest.Timestamp -> return
     *    4) Update resources
     */
    public static async Task LoadCrowdinStrings(CrowdinOptions options, ResourceDictionary destinationResources)
    {
        try
        {
            IDictionary<string, string>? translations;
            bool useNetwork = IsNetworkAllowed(options.NetworkPolicy);
            
            #region 1 - Cache disabled, network denied
            
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (!options.UseCache && !useNetwork) return;
            
            #endregion
            
            #region 2 - Cache disabled, network allowed
            
            if (!options.UseCache)
            {
                await СrowdinClient.Init(options.DistributionHash);
                
                translations =
                    await СrowdinClient.GetFileTranslations(
                        options.FileName, CurrentCulture.TwoLetterISOLanguageName);
                
                CopyResources(translations, destinationResources);
                return;
            }
            
            #endregion
            
            translations = ResourcesCacheManager.GetCachedCopy(options.FileName);
            
            #region 3 - Cache enabled, not saved yet, network denied
            
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (translations is null && !useNetwork) return;
            
            #endregion
            
            #region 4 - Cache enabled, not saved yet, network allowed
            
            if (translations is null)
            {
                await СrowdinClient.Init(options.DistributionHash);
                
                translations =
                    await СrowdinClient.GetFileTranslations(
                        options.FileName, CurrentCulture.TwoLetterISOLanguageName);
                
                CopyResources(translations, destinationResources);
                ResourcesCacheManager.SaveToCache(options.FileName, translations);
                
                return;
            }
            
            #endregion
            
            #region 5 - Cache enabled, file found
            
            CopyResources(translations, destinationResources);
            
            #region 5.1 - Network denied -> no check after cache copy
            
            if (!useNetwork) return;
            
            #endregion
            
            #region 5.2 - Network allowed -> update cache asynchronously
            
#pragma warning disable CS4014
            Task.Run(async () =>
#pragma warning restore CS4014
            {
                await СrowdinClient.Init(options.DistributionHash);
                if (!СrowdinClient.IsInitialized) return;
                
                DateTimeOffset remoteUpdateTime = СrowdinClient.Manifest!.Timestamp;
                bool isCacheUpToDate = ResourcesCacheManager.IsCacheUpToDate(options.FileName, remoteUpdateTime);
                
                if (!isCacheUpToDate)
                {
                    IDictionary<string, string> newResources =
                        await СrowdinClient.GetFileTranslations(
                            options.FileName, CurrentCulture.TwoLetterISOLanguageName);
                    
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        CopyResources(newResources, destinationResources);
                    });
                    
                    ResourcesCacheManager.SaveToCache(options.FileName, newResources);
                }
            });
            
            #endregion
            
            #endregion
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: Crowdin DynamicResourcesLoader: {0}", ex.Message);
        }
    }
    
    private static void CopyResources(IDictionary<string, string> source, ResourceDictionary destination)
    {
        foreach (KeyValuePair<string,string> kvp in source)
        {
            destination[kvp.Key] = kvp.Value;
        }
    }
    
    private static bool IsNetworkAllowed(NetworkPolicy policy)
    {
        return policy switch
        {
            NetworkPolicy.All => true,
            NetworkPolicy.Forbidden => false,
            
            NetworkPolicy.OnlyWiFi =>
                Connectivity.ConnectionProfiles
                    .Any(profile => profile is ConnectionProfile.WiFi),
            
            NetworkPolicy.OnlyCellular =>
                Connectivity.ConnectionProfiles
                    .Any(profile => profile is ConnectionProfile.Cellular),
            
            _ => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }
}