using LLM.Abstractions.Intefaces;
using LLM.OpenAI.Client.Factories;
using LLM.OpenAI.Client.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LLM.OpenAI.Client.Services
{
    internal class RequestBuilder(IOptions<LlmOptions> options, ToolCallHandler toolCallHandler)
    {
        private static readonly JsonSerializerOptions s_jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public async Task<StringContent> BuildRequestBody(IEnumerable<IChatMessage> messages, bool useStream)
        {
            Dictionary<string, object> requestBody = OpenAIFactory.CreateRequestBody(messages, useStream, options.Value);

            JsonElement[] tools = await toolCallHandler.GetToolsAsync();

            if(tools.Length > 0)
            {
               var toolProperties = OpenAIFactory.CreateToolCallRequestBodyProperties(tools);
                foreach(var kvp in toolProperties)
                {
                     requestBody[kvp.Key] = kvp.Value;
                }
            }

            string json = JsonSerializer.Serialize(requestBody, s_jsonOptions);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
