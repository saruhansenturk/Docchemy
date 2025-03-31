using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Docchemy.Services
{
    public class DocumentService
    {
        private readonly IChatCompletionService _chatCompletionService;

        public DocumentService(IChatCompletionService chatCompletionService)
        {
            _chatCompletionService = chatCompletionService;
        }

        public async Task GetMessageStreamAsync(string prompt, CancellationToken? cancellationToken = default!)
        {
            try
            {
                Console.WriteLine("\n🔵 AI Yanıtı Başlıyor:\n");

                await foreach (var response in _chatCompletionService.GetStreamingChatMessageContentsAsync(prompt))
                {

                    // Satır satır düzgün yazdır
                    Console.WriteLine(response?.ToString()?.Trim());
                }

                Console.WriteLine("\n✅ Yanıt Tamamlandı.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Hata: {ex.Message}");
            }
        }

    }
}
