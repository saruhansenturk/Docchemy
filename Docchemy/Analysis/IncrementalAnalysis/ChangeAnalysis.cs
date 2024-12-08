using System.Security.Cryptography;
using Docchemy.Assembler;

namespace Docchemy.Analysis.IncrementalAnalysis;

public class ChangeAnalysis
{
    private static string cacheFilePath = "cache.json";

    public static async Task AnalyzeProjectAsync(string solutionPath, Dictionary<string, string>? cache)
    {
        // get all .csproj files
        var csProjFiles = Directory.GetFiles(solutionPath, "*.csproj", SearchOption.AllDirectories).Where(t => !t.Contains("Docchemy.csproj"));

        foreach (var csProjFile in csProjFiles)
        {
            var projectDir = Path.GetDirectoryName(csProjFile);

            // get current context .cs files
            var csFiles = Directory.GetFiles(projectDir, "*.cs", SearchOption.AllDirectories);

            var filteredCsFiles = FilterCsFiles(csFiles);

            foreach (var csFile in filteredCsFiles)
            {
                string hash = await _ComputeFileHashAsync(csFile);

                if (cache.TryGetValue(csFile, out var cachedHash) && cachedHash == hash)
                {
                    Console.WriteLine($"No changes detected in {csFile}");
                }
                else
                {
                    await CommentReader.ReadCommentAsync(csFile);

                    Console.WriteLine($"Change detected in {csFile}, updating hash...");
                    cache[csFile] = hash;
                }
            }
        }
    }

    public static Dictionary<string, string>? LoadCache(string cacheFilePath)
    {
        if (!File.Exists(cacheFilePath))
            return new Dictionary<string, string>();

        var json = File.ReadAllText(cacheFilePath);
        return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
    }

    //save the hashed cache
    public static void SaveCache(string cacheFilePath, Dictionary<string, string> cache)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(cache, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(cacheFilePath, json);
    }

    private static async Task<string> _ComputeFileHashAsync(string filePath)
    {
        using var sha256 = SHA256.Create();
        await using var stream = File.OpenRead(filePath);
        var hashBytes = await sha256.ComputeHashAsync(stream);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }

    public static string[] FilterCsFiles(string[] allFiles) =>
        allFiles.Where(t =>
            t.EndsWith(".cs") &&
            !t.Contains("GlobalUsings") &&
            !t.Contains(".g.cs") &&
            !t.Contains(".AssemblyAttributes.cs") &&
            !t.Contains(".AssemblyInfo.cs"))
            .ToArray();
}