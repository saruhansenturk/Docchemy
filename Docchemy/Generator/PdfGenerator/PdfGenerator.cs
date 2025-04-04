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

            string htmlBody = Markdown.ToHtml(markdownContent);

            string htmlContent = $@"
                            <!DOCTYPE html>
                            <html>
                            <head>
                                <meta charset='UTF-8'>
                                <style>
                                    body {{
                                        font-family: 'DejaVu Sans', sans-serif;
                                        font-size: 14px;
                                        line-height: 1.6;
                                        padding: 20px;
                                    }}
                                    h1, h2, h3 {{
                                        color: #2c3e50;
                                    }}
                                    pre, code {{
                                        background-color: #f4f4f4;
                                        padding: 4px;
                                        border-radius: 4px;
                                        font-family: Consolas, monospace;
                                    }}
                                    ul {{
                                        margin-left: 20px;
                                    }}
                                </style>
                            </head>
                            <body>
                            {htmlBody}
                            </body>
                            </html>";
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
