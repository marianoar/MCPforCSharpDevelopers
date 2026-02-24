using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LLM.OpenAI.Client.Models.LLMResponse
{
    internal class ToolCall
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null;
        [JsonPropertyName("type")]
        public string Type { get; set; } = "function";
        [JsonPropertyName("function")]
        public FunctionCall Function { get; set; } = null!;

    }
}
