using LLM.Abstractions.Intefaces;
using LLM.OpenAI.Client.Options;
using LLM.OpenAI.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LLM.OpenAI.Client
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddOpenAIProvider(this IServiceCollection services,
                                                                Action<LlmOptions> configureOptions)
        {
            services.AddHttpClient();
            services.AddSingleton<ILlmClient, LlmClient>();
            services.Configure(configureOptions);

            services.AddSingleton<LlmHttpClient>();
            services.AddSingleton<ToolCallHandler>();
            services.AddSingleton<RequestBuilder>();
            services.AddSingleton<ResponseHandler>();
            services.AddSingleton<StreamResponseHandler>();

            return services;
        }


    }
}
