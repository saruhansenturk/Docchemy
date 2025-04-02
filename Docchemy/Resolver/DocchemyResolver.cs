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

namespace Docchemy.Resolver
{
    public static class DocchemyResolver
    {
        public static void AddDocchemyOpenAIBinding(this IServiceCollection services)
        {
            services.AddOpenAIChatCompletion(  
                modelId: "qwen/qwq-32b:free",
                openAIClient: new OpenAIClient(
                    credential: new ApiKeyCredential("API_KEY"),
                    options: new OpenAIClientOptions
                    {
                        Endpoint = new Uri("https://openrouter.ai/api/v1")
                    })
            );

            services.AddSingleton<DocumentService>();
            services.AddSingleton<PdfGenerator>();
            services.AddSingleton(typeof(IConverter), new BasicConverter(new PdfTools()));

        }
    }
}
