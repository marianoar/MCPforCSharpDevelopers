using LLM.Abstractions.Models;
using Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace LLM.Abstractions.Intefaces
{
    public interface IToolProvider
    {
        Task<IEnumerable<Tool>> GetToolsAsync();
        Task<Result<string>> ExecuteToolAsync(ToolExecuteArguments args);


    }
}
