using Docchemy.Analysis.ProjectAnalyzer;
using Docchemy.Resolver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDocchemyOpenAIBinding();
    })
    .Build();

//var serviceProvider = new ServiceCollection();

string solutionPath = @"C:\Users\srhn7\source\repos\Docchemy";
string cacheFilePath = @"C:\Users\srhn7\source\repos\Docchemy\cache.json";

//await ChangeAnalysis.AnalyzeProjectAsync(solutionPath, host.Services);


host.Run();