using Docchemy.Analysis.ProjectAnalyzer.CommentAnalyzer;
using Docchemy.Generator;
using Docchemy.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;

namespace Docchemy.Analysis.ProjectAnalyzer;

public class ChangeAnalysis
{
    public static async Task AnalyzeProjectAsync(string solutionPath, string? outPutPath, IServiceProvider serviceProvider)
    {
        var documentService = serviceProvider.GetRequiredService<DocumentService>();

        // get all .csproj files
        var csProjFiles = Directory.GetFiles(solutionPath, "*.csproj", SearchOption.AllDirectories)
            .Where(t => !t.Contains("Docchemy.csproj"));

        var analyzedList = new Dictionary<string, List<string>>();

        foreach (var csProjFile in csProjFiles)
        {
            var projectDir = Path.GetDirectoryName(csProjFile);

            // get current context .cs files
            var csFiles = Directory.GetFiles(projectDir, "*.cs", SearchOption.AllDirectories);

            var filteredCsFiles = FilterCsFiles(csFiles);

            var analyzedCsList = new List<string>();

            foreach (var csFile in filteredCsFiles)
            {

                // analyze all cs files and get all comments, class and method names
                //var analyzedText = await CommentReader.ReadCommentAsync(csFile);
                var retriever = new Analyzer();
                var analyzerResult = retriever.GetHierarchicalClassInfo(csFile);

                analyzedCsList.Add(analyzerResult);
            }

            analyzedList.TryAdd(csProjFile, analyzedCsList);
        }

        var sb = new StringBuilder();
        sb.AppendLine("Please generate a document based on the following project analysis:");
        sb.AppendLine();

        foreach (var project in analyzedList)
        {
            sb.AppendLine($"📂 Project: {project.Key}");

            foreach (var item in project.Value)
            {
                sb.AppendLine($"   - {item}");
            }

            sb.AppendLine();
        }

        string promptText = sb.ToString();

        await documentService.GetMessageStreamAsync(promptText, outPutPath);
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