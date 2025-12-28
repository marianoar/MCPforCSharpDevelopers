using System;
using System.Collections.Generic;
using System.Text;

namespace LLM.OpenAI.Client.Options
{
    public class LlmOptions
    {
        public string Model { get; set; } = "";

        public string BaseUrl { get; set; } = "";

        public string RelativeEndpoint { get; set; } = "";

        public TimeSpan? Timeout { get; set; }

        public string AuthenticationHeaderName { get; set; } = "";

        public string AuthenticationHeaderValue { get; set; } = "";



    }
}
