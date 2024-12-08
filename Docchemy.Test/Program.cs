

using System.Reflection;
using Docchemy.Assembler;
using Docchemy.Analysis.IncrementalAnalysis;
//using Docchemy.ReferenceSolver;


//var analyzedProjects = new HashSet<string>();

//await ReferenceSolver.AnalyzeProject(@"C:\Users\srhn7\source\repos\Docchemy\Docchemy.Test\Docchemy.Test.csproj", analyzedProjects);

string solutionPath = @"C:\Users\srhn7\source\repos\Docchemy";
string cacheFilePath = @"C:\Users\srhn7\source\repos\Docchemy\cache.json";

var cache = ChangeAnalysis.LoadCache(cacheFilePath);
await ChangeAnalysis.AnalyzeProjectAsync(solutionPath, cache);
ChangeAnalysis.SaveCache(cacheFilePath, cache);







