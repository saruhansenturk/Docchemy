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

                var markdown = await _chatCompletionService.GetChatMessageContentsAsync(prompt);

                string combinedContent = string.Join("\n", markdown.Select(m => m.Content?.ToString() ?? ""));
                
                Console.WriteLine("\n✅ Yanıt Tamamlandı.\n");

                _pdfGenerator.GeneratePdfFromMarkdown(combinedContent, outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Hata: {ex.Message}");
            }
        }
    }
}
