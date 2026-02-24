using LLM.Abstractions.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LLM.OpenAI.Client.Models
{
    //tool para el resto - function para openai
    internal class LlmFunction(JsonElement function, IToolProvider provider)
    {
        public JsonElement Function => function;
        public IToolProvider Provider => provider;

    }
}
