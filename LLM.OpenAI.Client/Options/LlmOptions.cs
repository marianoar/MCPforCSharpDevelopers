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

        public TimeSpan? Timeout { get; set; } = TimeSpan.FromMinutes(3);

        public string AuthenticationHeaderName { get; set; } = "Authorization";

        public string AuthenticationHeaderValue { get; set; } = "";

        public double Temperature { get; set; } = 0.5;

        public short MaxCompletionsTokens { get; set; } = 4096;

    }
}
