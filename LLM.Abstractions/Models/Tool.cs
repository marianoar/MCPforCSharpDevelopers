using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LLM.Abstractions.Models
{
    public  class Tool(string name, string description, JsonElement inputSchema)
    {
        public string Name => name;
        public string Description => description;
        public JsonElement InputSchema => inputSchema;
    }


}
