using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    credential: new ApiKeyCredential("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"),
                    options: new OpenAIClientOptions
                    {
                        Endpoint = new Uri("https://openrouter.ai/api/v1")
                    })
            );

            services.AddSingleton<DocumentService>();
        }
    }
}
