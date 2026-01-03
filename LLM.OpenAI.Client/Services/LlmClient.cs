using LLM.OpenAI.Client.Models;
using LLM.OpenAI.Client.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LLM.OpenAI.Client.Services
{
    public class LlmClient
    {
        private static readonly JsonSerializerOptions s_jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly IHttpClientFactory _factory;

        private readonly LlmOptions _options;

        public LlmClient(IHttpClientFactory factory, IOptions<LlmOptions> options)
        {
            _factory = factory;
            _options = options.Value;
        }

        public async Task StreamChatAsync(IEnumerable<Message> context, Action<string> handleResponse)
        {
            try
            {
                var client = CreateHttpClient();
                var request = BuildRequestBody(context, true);

                var response = await client.PostAsync(_options.RelativeEndpoint, request);

                if (!response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    handleResponse(string.Format("Error: {0} - {1}: {2}", response.StatusCode, response.ReasonPhrase, content));

                    return;
                }

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                string? line;

                while((line = await reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    if (line.StartsWith("data: "))
                        line = line.Substring("data: ".Length);
                    if (line == "[DONE]")
                        break;

                    handleResponse(line);
                }
            }
            catch (Exception ex)
            {

                handleResponse($"Error: {ex.Message}");
            }
        }
        public async Task<string> ChatAsync(IEnumerable<Message> context)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                var request = BuildRequestBody(context, false);

                var response = await httpClient.PostAsync(_options.RelativeEndpoint, request);

                if(!response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return string.Format("Error: {0} - {1}: {2}", response.StatusCode, response.ReasonPhrase, content);
                }

                string jsonResponse =  await  response.Content.ReadAsStringAsync();

                Console.WriteLine(jsonResponse);
                Console.WriteLine(" -- -- - - -- -- - ********* ------------ ");
                using var doc = JsonDocument.Parse(jsonResponse);

                JsonElement root = doc.RootElement;

                if(root.TryGetProperty("choices", out var choises) && choises.GetArrayLength()>0)
                {
                    JsonElement message = choises[0].GetProperty("message");
                    if(message.TryGetProperty("content", out var content))
                    {
                        return content.GetString() ?? "";
                    }
                }

                return "No content received from API";
            }
            catch (Exception ex)
            {
                return $"Error chat : {ex.Message}";
            }
        }
        public Message CreateSystemMessage(string content)
        {
            return new Message("system", content);
        }
        public Message CreateUserMessage(string content)
        {
            return new Message("user", content);
        }
        public Message CreateAssistantMessage(string content)
        {
            return new Message("assistant", content);
        }

        private HttpClient CreateHttpClient()
        {
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri(_options.BaseUrl);
            if (_options.Timeout.HasValue)
            {
                client.Timeout = _options.Timeout.Value;
            }

            client.DefaultRequestHeaders.Add(_options.AuthenticationHeaderName, _options.AuthenticationHeaderValue);
            
            return client;
        }

        private StringContent BuildRequestBody(IEnumerable<Message> messages, bool useStream)
        {
            Dictionary<string, object> requestBody = new()
            {
                [ "model"] =  _options.Model,
                [ "stream"] = useStream,
                [ "messages"] =  messages,
                [ "temperature"] = _options.Temperature,
                ["max_completion_tokens"] = _options.MaxCompletionsTokens,
            };

            string json = JsonSerializer.Serialize(requestBody, s_jsonOptions);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
