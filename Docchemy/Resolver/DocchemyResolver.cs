using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using DinkToPdf;
using Docchemy.Generator.PdfGenerator;
using Docchemy.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using OpenAI;
using Docchemy.Analysis.ProjectAnalyzer;

namespace Docchemy.Resolver
{
    public static class DocchemyResolver
    {
        public static async Task AddDocchemyOpenAIBinding(this IServiceCollection services)
        {
            if (!AskDocumentCreate())
            {
                return;
            }

            string solutionPath = FindSolutionDirectory();

            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(Path.Combine(AppContext.BaseDirectory, "runtimes", "win-x64", "native", "libwkhtmltox.dll"));

            services.AddOpenAIChatCompletion(  
                modelId: "qwen/qwq-32b:free",
                openAIClient: new OpenAIClient(
                    credential: new ApiKeyCredential("AP_KEY"),
                    options: new OpenAIClientOptions
                    {
                        Endpoint = new Uri("https://openrouter.ai/api/v1")
                    })
            );

            services.AddSingleton<DocumentService>();
            services.AddSingleton<PdfGenerator>();
            services.AddSingleton(typeof(IConverter), new BasicConverter(new PdfTools()));

            var serviceProvider = services.BuildServiceProvider();
            await ChangeAnalysis.AnalyzeProjectAsync(solutionPath, null, serviceProvider);
        }

        private static bool AskDocumentCreate()
        {
            Console.Write("Do you want to generate documentation? (Y/N): ");
            var input = Console.ReadLine()?.Trim().ToUpper();
            return input == "Y";
        }

        private static string? FindSolutionDirectory()
        {
            var dir = Directory.GetCurrentDirectory();

            while (dir != null)
            {
                if (Directory.GetFiles(dir, "*.sln").Any())
                {
                    return dir; // Solution klasörü bulundu
                }

                dir = Directory.GetParent(dir)?.FullName;
            }

            return null; // Solution bulunamadı
        }
    }
}
