using LLM.Abstractions.Intefaces;
using LLM.OpenAI.Client.Enums;
using LLM.OpenAI.Client.Factories;
using LLM.OpenAI.Client.Models;
using LLM.OpenAI.Client.Options;
using Microsoft.Extensions.Options;
using Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LLM.OpenAI.Client.Services
{
    internal class LlmClient(StreamResponseHandler streamResponseHandler, ResponseHandler responseHandler) : ILlmClient
    {
        public async Task<Result<string>> StreamChatAsync(
            List<IChatMessage> context,
            Action<string> handleResponse) => await streamResponseHandler.GetResponseAsync(context, handleResponse);

        public async Task<Result<string>> ChatAsync(
            List<IChatMessage> context) => await responseHandler.GetResponseAsync(context);

        public IChatMessage CreateSystemMessage(string content) => 
            OpenAIFactory.CreateMessage(ChatRole.System, content);

        public IChatMessage CreateUserMessage(string content) =>
            OpenAIFactory.CreateMessage(ChatRole.User, content);
        public IChatMessage CreateAssistantMessage(string content) =>
            OpenAIFactory.CreateMessage(ChatRole.Assistant, content);

    }
}
