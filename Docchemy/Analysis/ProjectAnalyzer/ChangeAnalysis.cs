using Docchemy.Analysis.ProjectAnalyzer.CommentAnalyzer;
using Docchemy.Generator;

namespace Docchemy.Analysis.ProjectAnalyzer;

public class ChangeAnalysis
{
    public static async Task AnalyzeProjectAsync(string solutionPath)
    {
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

        var blackboxClient = new BlackboxDocumenter(TimeSpan.FromSeconds(30));

        var blackboxAiResponse = await blackboxClient.DocumantateAsync(analyzedList);
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