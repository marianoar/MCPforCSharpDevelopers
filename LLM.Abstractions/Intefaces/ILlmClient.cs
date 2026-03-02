using Results;

namespace LLM.Abstractions.Intefaces
{
    public interface ILlmClient
    {
        Task<Result<string>> StreamChatAsync(List<IChatMessage> context, Action<string> handleResponse);

        Task<Result<string>> ChatAsync(List<IChatMessage> context);

        IChatMessage CreateAssistantMessage(string content);
        IChatMessage CreateSystemMessage(string content);
        IChatMessage CreateUserMessage(string content);
    }
}
