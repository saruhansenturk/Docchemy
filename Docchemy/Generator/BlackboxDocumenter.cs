using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Docchemy.Generator.DocumenterService;
using Docchemy.Request;

namespace Docchemy.Generator
{
    public class BlackboxDocumenter : DocumenterService.DocumenterService, IBlackboxDocumenter
    {
        private readonly TimeSpan _timeout;

        public BlackboxDocumenter(TimeSpan timeout)
        {
            _timeout = timeout;
        }

        public BlackboxDocumenter() : this(TimeSpan.FromSeconds(15))
        {
        }

        public override async Task<Documentation> DocumantateAsync(Dictionary<string, List<string>> analyzedClasses)
        {
            var stringBuilder = new StringBuilder();

            foreach (var analyzedProjectPair in analyzedClasses.Values.FirstOrDefault()!.ToList())
            {
                stringBuilder.AppendLine(analyzedProjectPair);
            }

            Documentation document = await GenerateDocumentationAsync(stringBuilder.ToString());

            return document;
        }

        private async Task<Documentation> GenerateDocumentationAsync(string analyzedClass)
        {
            try
            {
                var key = GenerateKey();
                using var httpClient = new HttpClient();
                httpClient.Timeout = _timeout;
                httpClient.DefaultRequestHeaders.UserAgent.Clear();
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.blackbox.ai");
                request.Headers.Referrer = new Uri("https://api.blackbox.ai/agent/DocumentAimS6xy0p");//todo get agent and replace it
                var requestBody = new Request.Request
                {
                    Messages =
                    new List<Message>
                    {
                        new Message
                            {
                            Id = key,
                            Content =$@"Codes and comment lines that I want to translate into the document - {analyzedClass}",
                            Role = "user"
                        }
                    },
                    Id = key,
                    PreviewToken = null,
                    UserId = null,
                    CodeModelMode = true,
                    AgentMode = new AgentMode
                    {
                        Mode = true,
                        Id = "DocumentAimS6xy0p",
                        Name = "DocumentAi"
                    },
                    TrendingAgentMode = new TrendingAgentMode { },
                    IsMicMode = false,
                    MaxTokens = 1024,
                    IsChromeExt = false,
                    GithubToken = null,
                    ClickedAnswer2 = false,
                    ClickedAnswer3 = false,
                    ClickedForceWebSearch = false,
                    VisitFromDelta = null
                };

                var json = JsonSerializer.Serialize(requestBody);

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request, CancellationToken.None);

                if(!response.IsSuccessStatusCode) return new Documentation(string.Empty);

                var responseBody = await response.Content.ReadAsStringAsync();

                var jsonResponse = Regex.Match(responseBody, @"\{(?:[^{}]|\{|\})*\}", RegexOptions.Multiline).Value;

                if (string.IsNullOrEmpty(jsonResponse))
                    return new Documentation(string.Empty);
                
                var aiResponse = JsonSerializer.Deserialize<Documentation>(jsonResponse);


                if(aiResponse == null) return new Documentation(string.Empty);

                aiResponse.IsSuccess = !string.IsNullOrEmpty(aiResponse.Document);


                return aiResponse;
            }
            catch(Exception e)
            {
                return new Documentation(string.Empty);
            }
        }

        private string GenerateKey()
        {
            const int length = 7;
            const string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string allCharacters = uppercaseLetters + lowercaseLetters + numbers;
            var random = new Random();
            var keyArray = new char[length];
            for (var i = 0; i < length; i++)
            {
                keyArray[i] = allCharacters[random.Next(allCharacters.Length)];
            }
            return new string(keyArray);
        }
    }
}
