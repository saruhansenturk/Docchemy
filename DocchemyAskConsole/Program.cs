using System;
using System.IO;
using Docchemy.Analysis.ProjectAnalyzer;
using Docchemy.Resolver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. Solution dizinini otomatik olarak almak
        string solutionPath = FindSolutionDirectory();


        if (solutionPath != null)
        {
            Console.WriteLine($"Solution dizini: {solutionPath}");
        }
        else
        {
            Console.WriteLine("❌ Solution bulunamadı!");
            return;
        }

        // Kullanıcıya dökümantasyon oluşturma isteği sorulacak
        Console.Write("Do you want to generate documentation? (Y/N): ");
        var input = Console.ReadLine()?.Trim().ToUpper();

        Console.Write("Where do you want to create this document?: ");
        var outputPath = Console.ReadLine()?.Trim();

        // Eğer kullanıcı "Y" derse, dökümantasyon işlemi başlatılacak
        if (input == "Y")
        {
            Console.WriteLine("Documentation generation started...");

            // 2. Host oluşturma ve DI yapılandırması
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDocchemyOpenAIBinding(); // Servisleri ekle
                })
                .Build();

            // 3. Dependency Injection üzerinden servis al
            var services = host.Services;

            // 4. Proje analizini başlat
            await ChangeAnalysis.AnalyzeProjectAsync(solutionPath, outputPath, services);

            Console.WriteLine("Documentation has been generated successfully.");
            host.Run();
        }
        else
        {
            Console.WriteLine("Documentation generation skipped.");
        }
    }

    public static string? FindSolutionDirectory()
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