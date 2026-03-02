using LLM.Abstractions.Intefaces;
using Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace LLM.Abstractions.Services
{
    internal class ChatClient(ILlmClient llmClient) : IChatClient
    {
        protected List<IChatMessage> Context { get; } = new();
        public void AddSystemMessage(string message)
        {
            Context.Add(llmClient.CreateSystemMessage(message));
        }

        public Task<Result<string>> ChatAsync(string message)
        {
            Context.Add(llmClient.CreateUserMessage(message));
            return llmClient.ChatAsync(Context);
        }
    }
}
