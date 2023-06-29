#nullable enable

namespace Crowdin.Maui.Infrastructure;

internal static class ResourcesCacheManager
{
    private static readonly string RootPath = Path.Combine(FileSystem.CacheDirectory, "crowdin");
    
    internal static bool IsCacheUpToDate(string filename, DateTimeOffset dateToCompare)
    {
        string filepath = GetFileFullPath(filename);
        
        var fileInfo = new FileInfo(filepath);
        if (!fileInfo.Exists) return false;
        
        return File.GetLastWriteTimeUtc(filepath) >= dateToCompare;
    }
    
    internal static IDictionary<string, string>? GetCachedCopy(string filename)
    {
        string filePath = GetFileFullPath(filename);
        var fileInfo = new FileInfo(filePath);
        if (!fileInfo.Exists) return null;
        
        using FileStream fileStream = fileInfo.OpenRead();
        var reader = new BinaryReader(fileStream);
        var outDictionary = new Dictionary<string, string>();
        
        int count = reader.ReadInt32();
        
        for (var i = 0; i < count; i++)
        {
            string key = reader.ReadString();
            string value = reader.ReadString();
            outDictionary.Add(key, value);
        }
        
        return outDictionary;
    }
    
    internal static void SaveToCache(string filename, IDictionary<string, string> resources)
    {
        if (resources.Count is 0) return;
        
        string filepath = GetFileFullPath(filename);
        EnsureDirectoryCreated(filepath);
        
        var fileInfo = new FileInfo(filepath);
        using FileStream fileStream = fileInfo.Open(FileMode.Create, FileAccess.Write);
        
        var binaryWriter = new BinaryWriter(fileStream);
        
        binaryWriter.Write(resources.Count);
        foreach (KeyValuePair<string,string> kvPair in resources)
        {
            binaryWriter.Write(kvPair.Key);
            binaryWriter.Write(kvPair.Value);
        }
        binaryWriter.Flush();
    }
    
    private static string GetFileFullPath(string filename)
    {
        return Path.Combine(RootPath, $"{filename}.bin");
    }
    
    private static void EnsureDirectoryCreated(string path)
    {
        if (path.EndsWith(".bin"))
        {
            path = Path.GetDirectoryName(path)
                   ?? throw new DirectoryNotFoundException(path);
        }
        
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}