
using LLM.OpenAI.Client.Models;
using LLM.OpenAI.Client.Options;
using LLM.OpenAI.Client.Services;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();

services.AddHttpClient();
services.Configure<LlmOptions>(options =>
{
    options.BaseUrl = "https://api.groq.com/";
    options.RelativeEndpoint = "openai/v1/chat/completions";
    options.Model = "moonshotai/kimi-k2-instruct-0905";
    options.Timeout = TimeSpan.FromMinutes(2);

    options.AuthenticationHeaderName = "Authorization";
    string apiKey = Environment.GetEnvironmentVariable("GroqApiKey") ?? "";
    options.AuthenticationHeaderValue = $"Bearer {apiKey}";
});

services.AddSingleton<LlmClient>();

var app = services.BuildServiceProvider();

var llmClient = app.GetRequiredService<LlmClient>();

List<Message> context = [];

context.Add(llmClient.CreateSystemMessage
    ("... aqui va el system prompt ..."));

context.Add(llmClient.CreateUserMessage(" ... user prompt ... "));

await llmClient.StreamChatAsync(context, response =>
{
    Console.Write(ChunckResponseProcessor.ProcessChunk(response));
});

Console.WriteLine();

