using LLM.Abstractions.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LLM.OpenAI.Client.Models
{
    internal class Message(string role, string content): IChatMessage
    {
        [JsonPropertyName("role")]
        public string Role => role;
        [JsonPropertyName("content")]
        public string Content => content;

    }
}
