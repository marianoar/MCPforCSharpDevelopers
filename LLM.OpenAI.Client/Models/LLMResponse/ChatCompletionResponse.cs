using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LLM.OpenAI.Client.Models.LLMResponse
{
    internal class ChatCompletionResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;
        [JsonPropertyName("model")]
        public string Model { get; set; } = null!;
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; } = null!;
        [JsonPropertyName("usage")]
        public Usage Usage { get; set; } = null!;
    }
}
