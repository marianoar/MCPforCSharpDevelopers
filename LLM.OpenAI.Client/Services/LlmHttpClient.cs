using LLM.OpenAI.Client.Options;
using Microsoft.Extensions.Options;
using Results;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace LLM.OpenAI.Client.Services
{
    internal class LlmHttpClient(IHttpClientFactory factory, IOptions<LlmOptions> options)
    {
        public async Task<Result<string>> SendRequestAsync(StringContent request)
        {
            HttpClient client = CreateHttpClient();

            HttpResponseMessage response = await client.PostAsync(options.Value.RelativeEndpoint, request);

            if (!response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return Result<string>.Fail(string.Format("Error: {0} - {1}: {2}", response.StatusCode, response.ReasonPhrase, content));
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return Result<string>.Ok(jsonResponse);
        }
        public HttpClient CreateHttpClient()
        {
            var client = factory.CreateClient();
            client.BaseAddress = new Uri(options.Value.BaseUrl);
            if (options.Value.Timeout.HasValue)
            {
                client.Timeout = options.Value.Timeout.Value;
            }

            client.DefaultRequestHeaders.Add(options.Value.AuthenticationHeaderName, options.Value.AuthenticationHeaderValue);

            return client;
        }
    }
}
