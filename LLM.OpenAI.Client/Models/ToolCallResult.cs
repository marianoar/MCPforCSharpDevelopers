using LLM.Abstractions.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LLM.OpenAI.Client.Models
{
    internal class ToolCallResult(string toolCallId, string content) : IChatMessage
    {
        [JsonPropertyName("role")]
        public string Role => "tool";

        [JsonPropertyName("tool_call_id")]
        public string ToolCallId => toolCallId;

        [JsonPropertyName("content")]
        public string Content => content;
    }
}
