using LLM.Abstractions.Intefaces;
using LLM.Abstractions.Models;
using Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace ToolProviders.Weather
{
    internal class WeatherToolProvider : IToolProvider
    {
        const string ToolName = "get_current_weather";
        public async Task<Result<string>> ExecuteToolAsync(ToolExecuteArguments args)
        {
            try
            {
                if (args.Tool == ToolName)
                {
                    var location = args.Arguments.GetProperty("location").GetString();

                    string unit = "celsius";

                    if (args.Arguments.TryGetProperty("unit", out var unitProp))
                    {
                        unit = unitProp.GetString() ?? "celsius";
                    }

                    await Task.Delay(500); // Simulate async work
                    var temp = Random.Shared.Next(-15, 35);

                    if (unit == "fahrenheit")
                    {
                        temp = (int)(temp * 9 / 5 + 32);
                    }

                    string unitSymbol = unit.ToLower() == "fahrenheit" ? "°F" : "°C";

                    var result = $"The current temperature in {location} is {temp}{unitSymbol}.";

                    return Result<string>.Ok(result);
                }
                return Result<string>.Fail($"Herramienta desconocida: {args.Tool}");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Error executing tool: {args.Tool} - {ex.Message}");
            }
        }

        public Task<IEnumerable<Tool>> GetToolsAsync()
        {
            var tools = new List<Tool>
            {
               new Tool(
                    name: ToolName,
                    description: "Get the current weather for a given location. Optionally specify the unit (celsius or fahrenheit).",
                    inputSchema: JsonSerializer.SerializeToElement(new
                    {
                        type = "object",
                        properties = new
                        {
                            location = new
                            {
                                type = "string",
                                description = "The location to get the weather for."
                            },
                            unit = new
                            {
                                type = "string",
                                description = "The unit for temperature (celsius or fahrenheit). Optional, defaults to celsius.",
                                @enum = new[] { "celsius", "fahrenheit" }
                            }
                        },
                        required = new[] { "location" }
                    }
                    )
               )
            };
            return Task.FromResult<IEnumerable<Tool>>(tools);
        }
    }
}
