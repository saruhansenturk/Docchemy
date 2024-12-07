

using System.Reflection;
using Docchemy.Assembler;
using Docchemy.Analysis.IncrementalAnalysis;
//using Docchemy.ReferenceSolver;


//var analyzedProjects = new HashSet<string>();

//await ReferenceSolver.AnalyzeProject(@"C:\Users\srhn7\source\repos\Docchemy\Docchemy.Test\Docchemy.Test.csproj", analyzedProjects);


var cache = ChangeAnalysis.LoadCache();
ChangeAnalysis.AnalyzeProject(@"C:\Users\srhn7\source\repos\Docchemy\Docchemy.Test\Docchemy.Test.csproj", cache);
ChangeAnalysis.SaveCache(cache);







