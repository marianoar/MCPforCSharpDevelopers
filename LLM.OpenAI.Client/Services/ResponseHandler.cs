using LLM.Abstractions.Intefaces;
using LLM.OpenAI.Client.Factories;
using LLM.OpenAI.Client.Models.LLMResponse;
using LLM.OpenAI.Client.Resources;
using Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace LLM.OpenAI.Client.Services
{
    internal class ResponseHandler (RequestBuilder requestBuilder, LlmHttpClient llmHttpClient, ToolCallHandler toolCallHandler)
    {
        public async Task<Result<string>> GetResponseAsync(List<IChatMessage> context)
        {
            int maxIterations = 10;
            int currentIteration = 0;

            StringBuilder fullResponse = new StringBuilder();

            try
            {
                while (currentIteration < maxIterations)
                {
                    currentIteration++;

                    StringContent requestBody = await requestBuilder.BuildRequestBody(context, false);

                    Result<string> sendRequestResult = await llmHttpClient.SendRequestAsync(requestBody);

                    if(sendRequestResult.IsFailure)
                    {
                        return Result<string>.Fail(sendRequestResult.ErrorMessage);
                    }

                    ChatCompletionResponse response = OpenAIFactory.CreateChatCompletionResponse(sendRequestResult.Value);

                    ChoiceMessage message = response.Choices[0].Message;

                    context.Add(message);
                    // puede ser que tenga respuesta, tool calls o ambas. Ademas puede ser que la tool call ejecute en paralelo varias.
                    if(!string.IsNullOrWhiteSpace(message.Content))
                        fullResponse.AppendLine(message.Content);
                    if (message.ToolCalls?.Count > 0)
                    {
                        var results = await Task.WhenAll(message.ToolCalls.Select(toolCall => toolCallHandler.ExecuteToolAsync(toolCall)));

                        context.AddRange(results);
                    }
                    else
                    {
                        break;
                    }
                }
                if(currentIteration >= maxIterations)
                    return Result<string>.Fail(Messages.MaximumIterationReached);

                return Result<string>.Ok(fullResponse.ToString());
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(ex.Message);
            }
    }
}
