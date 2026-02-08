using LLM.Abstractions.Intefaces;
using LLM.OpenAI.Client.Models;
using LLM.OpenAI.Client.Options;
using Microsoft.Extensions.Options;
using Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LLM.OpenAI.Client.Services
{
    internal class LlmClient : ILlmClient
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

        public async Task<Result> StreamChatAsync(List<IChatMessage> context, Action<string> handleResponse)
        {
            try
            {
                var client = CreateHttpClient();
                var request = BuildRequestBody(context, true);

                var response = await client.PostAsync(_options.RelativeEndpoint, request);

                if (!response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return Result.Fail(string.Format("Error: {0} - {1}: {2}", response.StatusCode, response.ReasonPhrase, content));
                    
                }

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                string? line;

                StringBuilder fullResponse = new StringBuilder();

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    if (line.StartsWith("data: "))
                        line = line.Substring("data: ".Length);
                    if (line == "[DONE]")
                        break;

                    string content = ChunckResponseProcessor.ProcessChunk(line);
                    fullResponse.Append(content);

                    handleResponse(content);
                }

                context.Add(CreateAssistantMessage(fullResponse.ToString()));
                return Result.Ok();
            }
            catch (Exception ex)
            {

                return Result.Fail($"Error: {ex.Message}");
            }
        }
        public async Task<Result> ChatAsync(List<IChatMessage> context)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                var request = BuildRequestBody(context, false);

                var response = await httpClient.PostAsync(_options.RelativeEndpoint, request);

                if(!response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return Result.Fail(string.Format("Error: {0} - {1}: {2}", response.StatusCode, response.ReasonPhrase, content));
                }

                string jsonResponse =  await  response.Content.ReadAsStringAsync();

                //Console.WriteLine(jsonResponse);
                //Console.WriteLine(" -- -- - - -- -- - ********* ------------ ");
                using var doc = JsonDocument.Parse(jsonResponse);

                JsonElement root = doc.RootElement;

                if(root.TryGetProperty("choices", out var choises) && choises.GetArrayLength()>0)
                {
                    JsonElement message = choises[0].GetProperty("message");
                    if(message.TryGetProperty("content", out var content))
                    {
                        string fullResponse = content.GetString() ?? "";
                        context.Add(CreateAssistantMessage(fullResponse));

                        return Result.Ok();
                    }
                }

                return Result.Fail("No content received from API");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error chat : {ex.Message}");
            }
        }
        public IChatMessage CreateSystemMessage(string content)
        {
            return new Message("system", content);
        }
        public IChatMessage CreateUserMessage(string content)
        {
            return new Message("user", content);
        }
        public IChatMessage CreateAssistantMessage(string content)
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

        private StringContent BuildRequestBody(IEnumerable<IChatMessage> messages, bool useStream)
        {
            var messageObjects =  messages.Select( m=> 
                                    JsonSerializer.SerializeToElement( m, m.GetType(), s_jsonOptions)).ToArray();
            Dictionary<string, object> requestBody = new()
            {
                [ "model"] =  _options.Model,
                [ "stream"] = useStream,
                [ "messages"] =  messageObjects,
                [ "temperature"] = _options.Temperature,
                ["max_completion_tokens"] = _options.MaxCompletionsTokens,
            };

            string json = JsonSerializer.Serialize(requestBody, s_jsonOptions);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
