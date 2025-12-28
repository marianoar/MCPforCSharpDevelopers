using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LLM.OpenAI.Client.Services
{
    public static class ChunckResponseProcessor
    {
        public static string ProcessChunk(string chunck)
        {
			try
			{
				using var document = JsonDocument.Parse(chunck);
                var root = document.RootElement;

                if(root.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
                {
                    var delta = choices[0].GetProperty("delta");

                    if(delta.TryGetProperty("content", out var content))
                    {
                        return content.GetString() ?? "";
                    }
                }
            }
			catch (JsonException)
			{

				return chunck;
			}
            return "";
        }
    }
}
