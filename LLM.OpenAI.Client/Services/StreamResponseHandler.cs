using LLM.Abstractions.Intefaces;
using LLM.OpenAI.Client.Enums;
using LLM.OpenAI.Client.Factories;
using LLM.OpenAI.Client.Options;
using LLM.OpenAI.Client.Resources;
using Microsoft.Extensions.Options;
using Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;

namespace LLM.OpenAI.Client.Services
{
    internal class StreamResponseHandler(RequestBuilder requestBuilder, LlmHttpClient llmHttpClient, IOptions<LlmOptions> options)
    {
        public async Task<Result<string>> GetResponseAsync(List<IChatMessage> context, Action<string> handleResponse)
        {
            try
            {
                var client = llmHttpClient.CreateHttpClient();

                var request = await requestBuilder.BuildRequestBody(context, true);

                var response = await client.PostAsync(options.Value.RelativeEndpoint, request);

                if (!response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return Result<string>.Fail(string.Format("Error: {0} - {1}: {2}", response.StatusCode, response.ReasonPhrase, content));
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

                string responseContent = fullResponse.ToString();
                
                context.Add(OpenAIFactory.CreateMessage(ChatRole.Assistant, responseContent));

                return Result<string>.Ok(responseContent);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(string.Format(Messages.StreamingErrorTemplate, ex.Message));
            }
        }
    }
}
