
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
    ("Eres un profesor de matematicas."));

context.Add(llmClient.CreateUserMessage("Explica de manera breve, clara y concisa porque no se puede dividir por cero."));

//await llmClient.StreamChatAsync(context, response =>
//{
//    Console.WriteLine(ChunckResponseProcessor.ProcessChunk(response));
//});

//Console.WriteLine();

Console.WriteLine("Respuesta completa:");
string response = await llmClient.ChatAsync(context);
Console.WriteLine(response);

