using LLM.Abstractions.Intefaces;
using LLM.Abstractions.Models;
using LLM.OpenAI.Client.Enums;
using LLM.OpenAI.Client.Models;
using LLM.OpenAI.Client.Models.LLMResponse;
using LLM.OpenAI.Client.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LLM.OpenAI.Client.Factories
{
    internal class OpenAIFactory
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public static Message CreateMessage(ChatRole role, string content) =>
            role switch
            { ChatRole.System => new Message ("system", content),
              ChatRole.User => new Message ("user",content ),
              ChatRole.Assistant => new Message( "assistant", content),
              _ => throw new ArgumentOutOfRangeException( nameof(role), role, null)
            };

        public static JsonElement CreateFunction(Tool tool) =>
            JsonSerializer.SerializeToElement(new
            {
                type = "function",
                function = new
                {
                    name = tool.Name,
                    description = tool.Description,
                    parameters = tool.InputSchema
                }
            });

        public static ToolExecuteArguments CreateToolExecuteArguments(ToolCall toolCall)
        { JsonElement arguments = JsonDocument.Parse(toolCall.Function.Arguments).RootElement;
            return new (toolCall.Id, toolCall.Function.Name, arguments);
        }

        public static Dictionary<string, object> CreateRequestBody(
            IEnumerable<IChatMessage> messages,
            bool useStream,
            LlmOptions options)
        {
            var messageObjects = messages.Select(m=>
             JsonSerializer.SerializeToElement(m, m.GetType(), _jsonOptions));

            return new()
            {
                ["model"] = options.Model,
                ["stream"] = useStream,
                ["messages"] = messageObjects,
                ["temperature"] = options.Temperature,
                ["max_tokens"] = options.MaxCompletionsTokens,
            };
        }

        public static Dictionary<string, object> CreateToolCallRequestBodyProperties(
            JsonElement[] tools) => new()
            {
                ["tools"] = tools,
                ["tool_choice"] = "auto"
            };

        public static ChatCompletionResponse CreateChatCompletionResponse(string jsonResponse)
            => JsonSerializer.Deserialize<ChatCompletionResponse>(jsonResponse, _jsonOptions)!;
     }
}
