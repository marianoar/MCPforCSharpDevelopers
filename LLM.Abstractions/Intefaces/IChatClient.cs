using Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace LLM.Abstractions.Intefaces
{
    internal interface IChatClient
    {
        void AddSystemMessage(string message);
        Task<Result<string>> ChatAsync(string message);
    }
}
