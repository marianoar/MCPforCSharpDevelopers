namespace LLM.Abstractions.Intefaces
{
    public interface IChatMessage
    {
        string Role { get; }
        string? Content { get; }
    }
}
