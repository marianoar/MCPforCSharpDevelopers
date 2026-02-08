
using ConsoleClient;
using LLM.OpenAI.Client.Models;
using LLM.OpenAI.Client.Options;
using LLM.OpenAI.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.Services.AddHttpClient();
//builder.Logging.AddFilter("System.Net.Http", LogLevel.Warning); en caso que quiera subir el nivel
builder.Services.Configure<LlmOptions>(options =>
{
    options.BaseUrl = "https://api.groq.com/";
    options.RelativeEndpoint = "openai/v1/chat/completions";
    options.Model = "moonshotai/kimi-k2-instruct-0905";
    options.Timeout = TimeSpan.FromMinutes(2);

    options.AuthenticationHeaderName = "Authorization";
    //Host toma las variables de ambiente como parte de la configuracion
    string apiKey = builder.Configuration["GroqApiKey"] ?? "";
    options.AuthenticationHeaderValue = $"Bearer {apiKey}";
});

builder.Services.AddSingleton<LlmClient>();
builder.Services.AddSingleton<ChatClient>();

var app = builder.Build();

//var llmClient = app.Services.GetRequiredService<LlmClient>();
var chatClient = app.Services.GetRequiredService<ChatClient>();

//List<Message> context = [];

//context.Add(llmClient.CreateSystemMessage
//    ("Eres un profesor de matematicas."));

//context.Add(llmClient.CreateUserMessage("Explica de manera breve, clara y concisa porque no se puede dividir por cero."));

//await llmClient.StreamChatAsync(context, response =>
//{
//    Console.WriteLine(ChunckResponseProcessor.ProcessChunk(response));
//});

//Console.WriteLine();


//string response = await llmClient.ChatAsync(context);

await chatClient.StartAsync();


