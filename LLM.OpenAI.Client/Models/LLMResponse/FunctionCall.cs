using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LLM.OpenAI.Client.Models.LLMResponse
{
    internal class FunctionCall
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
        [JsonPropertyName("arguments")]
        public string Arguments { get; set; } = null!;
    }
}
