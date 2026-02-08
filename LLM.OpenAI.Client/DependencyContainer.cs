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

            return services;
        }


    }
}
