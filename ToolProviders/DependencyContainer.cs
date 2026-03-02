using LLM.Abstractions.Intefaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ToolProviders.Weather;

namespace ToolProviders
{
    internal static class DependencyContainer
    {
        public static IServiceCollection AddWeatherToolProvider(this IServiceCollection services)
        {
            return services.AddSingleton<IToolProvider, WeatherToolProvider>();
        }
    }
}
