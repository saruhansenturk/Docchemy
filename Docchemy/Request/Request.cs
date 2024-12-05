using System.Text.Json.Serialization;

namespace Docchemy.Request
{
    public class Request
    {
        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("previewToken")]
        public object? PreviewToken { get; set; }

        [JsonPropertyName("userId")]
        public object? UserId { get; set; }

        [JsonPropertyName("codeModelMode")]
        public bool CodeModelMode { get; set; }

        [JsonPropertyName("agentMode")]
        public AgentMode AgentMode { get; set; }

        [JsonPropertyName("trendingAgentMode")]
        public TrendingAgentMode TrendingAgentMode { get; set; }

        [JsonPropertyName("isMicMode")]
        public bool IsMicMode { get; set; }

        [JsonPropertyName("maxTokens")]
        public int MaxTokens { get; set; }

        [JsonPropertyName("isChromeExt")]
        public bool IsChromeExt { get; set; }

        [JsonPropertyName("githubToken")]
        public object? GithubToken { get; set; }

        [JsonPropertyName("clickedAnswer2")]
        public bool ClickedAnswer2 { get; set; }

        [JsonPropertyName("clickedAnswer3")]
        public bool ClickedAnswer3 { get; set; }

        [JsonPropertyName("clickedForceWebSearch")]
        public bool ClickedForceWebSearch { get; set; }

        [JsonPropertyName("visitFromDelta")]
        public object? VisitFromDelta { get; set; }
    }

    public class AgentMode
    {
        [JsonPropertyName("mode")]
        public bool Mode { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }
    }

    public class TrendingAgentMode
    {
    }
}
