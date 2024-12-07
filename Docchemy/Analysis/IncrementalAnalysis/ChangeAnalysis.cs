using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace Docchemy.Analysis.IncrementalAnalysis;

public class ChangeAnalysis
{
    private static string cacheFilePath = "cache.json";

    public static async Task AnalyzeProject(string projectPath, AnalysisCache cache)
    {
        MSBuildLocator.RegisterDefaults();

        Project project = null;

        try
        {
            using var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (sender, e) => Console.WriteLine($"Workspace Error: {e.Diagnostic}");

            project = await workspace.OpenProjectAsync(projectPath);
            Console.WriteLine($"Project '{project.Name}' loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        var documents = project.Documents;


        bool hasChange = false;

        foreach (var document in documents)
        {
            var filePath = document.FilePath;
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                continue;

            string currentHash = _ComputeFileHash(filePath);

            if (cache.FileHashes.TryGetValue(filePath, out var previousHash) && previousHash == currentHash)
            {
                Console.WriteLine($"Skipping unchanged file: {filePath}");
                continue;
            }

            Console.WriteLine($"Analyzing file: {filePath}");
            await PerformFileAnalysis(document);

            cache.FileHashes[filePath] = currentHash;
            hasChange = true;
        }
    }
    
    private static async Task PerformFileAnalysis(Document document)
    {
        var syntaxTree = await document.GetSyntaxTreeAsync();
        Console.WriteLine($"Completed analysis for: {document.FilePath}");
        Console.WriteLine($"Syntax Tree: {syntaxTree}");
    }


    public static AnalysisCache LoadCache()
    {
        if (!File.Exists(cacheFilePath)) return new AnalysisCache();


        var json = File.ReadAllText(cacheFilePath);
        return JsonSerializer.Deserialize<AnalysisCache>(json) ?? new AnalysisCache();

    }

    public static void SaveCache(AnalysisCache cache)
    {
        string json = JsonSerializer.Serialize(cache, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(cacheFilePath, json);
    }

    private static async Task _PerformAnalysis(string projectPath)
    {
        Console.WriteLine($"Documentation generated for {projectPath}");
    }

    private static string _ComputeFileHash(string filePath)
    {
        using var sha256 = SHA256.Create();
        using var stream = File.OpenRead(filePath);
        var hashBytes = sha256.ComputeHash(stream);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }
}
public class ProjectCacheEntry
{
    public string Hash { get; set; }
    public DateTime LastAnalyzed { get; set; }
}

public class AnalysisCache
{
    public Dictionary<string, string> FileHashes { get; set; } = new Dictionary<string, string>();
}