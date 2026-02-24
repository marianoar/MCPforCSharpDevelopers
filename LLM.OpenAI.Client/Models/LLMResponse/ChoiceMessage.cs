using LLM.Abstractions.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LLM.OpenAI.Client.Models.LLMResponse
{
    internal class ChoiceMessage : IChatMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = null!;

        [JsonPropertyName("content")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // content can be null when the message is a tool call, in which case the content is not included in the response
        public string? Content { get; set; }
        //no todos los llm pueden ejecutar herramientas en paralelo
        [JsonPropertyName("tool_calls")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ToolCall>? ToolCalls { get; set; }
    }
}
