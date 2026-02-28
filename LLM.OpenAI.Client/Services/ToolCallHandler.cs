using LLM.Abstractions.Intefaces;
using LLM.Abstractions.Models;
using LLM.OpenAI.Client.Factories;
using LLM.OpenAI.Client.Models;
using LLM.OpenAI.Client.Models.LLMResponse;
using Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LLM.OpenAI.Client.Services
{
    internal class ToolCallHandler(IEnumerable<IToolProvider> providers)
    {
        private Dictionary<string, LlmFunction>? _tools = null;

        private JsonElement[]? _functions;

        public async Task<JsonElement[]> GetToolsAsync()
        {
            if(_tools is null)
            {
                await ProcessProvidersAsync();
            }
            return _functions!;
        }
        public async Task<ToolCallResult> ExecuteToolAsync(ToolCall toolCall)
        {
            ToolExecuteArguments args = OpenAIFactory.CreateToolExecuteArguments(toolCall);

            Result<string> toolCallResult = await _tools![args.Tool].Provider.ExecuteToolAsync(args);

            string content = toolCallResult.IsSuccess ? toolCallResult.Value : toolCallResult.Error.Message;

            return new ToolCallResult(args.MessageId, content);
        }
        private async Task ProcessProvidersAsync()
        {
            var tools = new Dictionary<string, LlmFunction>(
                StringComparer.OrdinalIgnoreCase
                );
            foreach (var provider in providers)
            {
                var providerTools = await provider.GetToolsAsync();

                foreach (var tool in providerTools)
                {
                    if (!tools.ContainsKey(tool.Name))
                    {
                        tools.Add(tool.Name, new LlmFunction(
                            OpenAIFactory.CreateFunction(tool),
                            provider
                            ));
                    }
                }
            }
            _tools = tools;
            _functions = [.. tools?.Select(t => t.Value.Function) ?? []];
        }

    }
}
