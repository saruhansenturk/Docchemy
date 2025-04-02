using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Markdig;

namespace Docchemy.Generator.PdfGenerator
{
    public class PdfGenerator
    {
        //private readonly IConverter _pdfConverter;

        //public PdfGenerator(IConverter pdfConverter)
        //{
        //    _pdfConverter = pdfConverter;
        //}

        public void GeneratePdfFromMarkdown(string markdownContent, string? outputPath)
        {
            if (string.IsNullOrEmpty(outputPath))
            {
                string userDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                outputPath = Path.Combine(userDocuments, "AI_Document_Output.pdf");
            }

            var converter = new BasicConverter(new PdfTools());

            string htmlContent = Markdown.ToHtml(markdownContent);

            var pdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = new()
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Out = outputPath
                },
                Objects =
                {
                    new ObjectSettings
                    {
                        HtmlContent = htmlContent
                    }
                }
            };

            converter.Convert(pdfDocument);
            Console.WriteLine($"✅ PDF başarıyla oluşturuldu: {outputPath}");
        }
    }
}
