using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LLM.Abstractions.Models
{
    public class ToolExecuteArguments(string messageId, string tool, JsonElement arguments)
    {
        public string MessageId => messageId;
        public string Tool => tool;
        public JsonElement Arguments => arguments;
    }
}
