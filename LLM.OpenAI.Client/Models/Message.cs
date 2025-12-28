using System;
using System.Collections.Generic;
using System.Text;

namespace LLM.OpenAI.Client.Models
{
    public class Message(string role, string content)
    {
        public string Role => role;
        public string Content => content;

    }
}
