using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LLM.OpenAI.Client.Models.LLMResponse
{
    internal class Usage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; };
        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }
}
