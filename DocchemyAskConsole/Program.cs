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
        string solutionPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;

        // Kullanıcıya dökümantasyon oluşturma isteği sorulacak
        Console.Write("Do you want to generate documentation? (Y/N): ");
        var input = Console.ReadLine()?.Trim().ToUpper();

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
            await ChangeAnalysis.AnalyzeProjectAsync(solutionPath, services);

            Console.WriteLine("Documentation has been generated successfully.");
            host.Run();
        }
        else
        {
            Console.WriteLine("Documentation generation skipped.");
        }
    }
}