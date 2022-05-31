
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using JetBrains.Annotations;
using Resx.Resources;

using Crowdin.Xamarin.Forms.Infrastructure;
using Crowdin.Xamarin.Forms.Models;

#nullable enable

namespace Crowdin.Xamarin.Forms
{
    [PublicAPI]
    public static class СrowdinClient
    {
        public static bool IsInitialized { get; private set; }
        
        public static DistributionManifest? Manifest { get; private set; }
        
        private static string mBaseUrl = null!;
        
        private static readonly HttpClient HttpClient = new();
        private static readonly Dictionary<string, string> EmptyDictionary = new();
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            Converters =
            {
                new UnixTimeConverter()
            }
        };
        
        public static async Task Init(string distributionHash)
        {
            if (IsInitialized) return;
            mBaseUrl = $"https://distributions.crowdin.net/{distributionHash}";
            
            Manifest = await GetManifest();
            if (Manifest != null)
            {
                IsInitialized = true;
            }
        }
        
        private static async Task<DistributionManifest?> GetManifest()
        {
            if (!SystemHelpers.IsNetworkConnected()) return null;
            
            HttpResponseMessage response = await HttpClient.GetAsync($"{mBaseUrl}/manifest.json");
            Stream responseStream = await response.Content.ReadAsStreamAsync();
            
            return await JsonSerializer.DeserializeAsync<DistributionManifest>(responseStream, SerializerOptions);
        }
        
        public static async Task<IDictionary<string, string>> GetFileTranslations(string inFilename, string languageCode)
        {
            if (Manifest is null ||
                !SystemHelpers.IsNetworkConnected() ||
                Manifest.Languages.All(code => code != languageCode))
            {
                return EmptyDictionary;
            }
            
            if (!inFilename.StartsWith("/"))
            {
                inFilename = $"/{inFilename}";
            }
            
            var url = $"{mBaseUrl}/content/{languageCode}{inFilename}";
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            
            await using Stream rawResponseStream = await response.Content.ReadAsStreamAsync();
            Stream decompressedStream = await DecompressStream(rawResponseStream);
            
            using var resxReader = new ResXResourceReader(decompressedStream);
            return resxReader
                .Cast<DictionaryEntry>()
                .ToDictionary(entry => entry.Key.ToString(), entry => entry.Value.ToString());
        }
        
        private static async Task<Stream> DecompressStream(Stream inStream)
        {
            var outStream = new MemoryStream();
            await using var gzipStream = new GZipStream(inStream, CompressionMode.Decompress);
            
            await gzipStream.CopyToAsync(outStream);
            outStream.Position = 0;
            return outStream;
        }
    }
}