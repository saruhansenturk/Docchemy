using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Docchemy.Generator.PdfGenerator;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Docchemy.Services
{
    public class DocumentService
    {
        private readonly IChatCompletionService _chatCompletionService;
        private readonly PdfGenerator _pdfGenerator;

        public DocumentService(IChatCompletionService chatCompletionService, PdfGenerator pdfGenerator)
        {
            _chatCompletionService = chatCompletionService;
            _pdfGenerator = pdfGenerator;
        }

        public async Task GetMessageStreamAsync(string prompt, string outputPath, CancellationToken? cancellationToken = default!)
        {
            try
            {
                Console.WriteLine("\n🔵 AI Yanıtı Başlıyor:\n");

                var markdownBuilder = new StringBuilder();
                markdownBuilder.AppendLine("# AI Yanıtı\n");
                markdownBuilder.AppendLine("### Üretilen İçerik\n");
                
                await foreach (var response in _chatCompletionService.GetStreamingChatMessageContentsAsync(prompt))
                {
                    string? responseText = response?.ToString()?.Trim();
                    if (string.IsNullOrEmpty(responseText)) continue;

                    Console.WriteLine(responseText);

                    // Satır satır eklemek yerine birleştir
                    markdownBuilder.Append(responseText + " ");
                }

                Console.WriteLine("\n✅ Yanıt Tamamlandı.\n");

                string rawMarkdown = markdownBuilder.ToString();

                // 1. Başlıklar ile içerik bitişikse ayır (örn: ### BaşlıkBu içerik)
                rawMarkdown = Regex.Replace(rawMarkdown, @"(#+ .+?)([^\n#])", "$1\n$2");

                // 2. --- ile içerik bitişikse ayır
                rawMarkdown = Regex.Replace(rawMarkdown, @"(-{3,})([^\n])", "$1\n$2");

                // 3. Her başlığın kendi satırında olmasını sağla
                rawMarkdown = Regex.Replace(rawMarkdown, @"(?<!\n)(#+\s?)", "\n$1");

                markdownBuilder.Clear();
                markdownBuilder.Append(rawMarkdown);


                _pdfGenerator.GeneratePdfFromMarkdown(rawMarkdown, outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Hata: {ex.Message}");
            }
        }

    }
}
