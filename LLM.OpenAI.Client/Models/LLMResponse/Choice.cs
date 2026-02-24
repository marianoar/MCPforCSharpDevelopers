using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LLM.OpenAI.Client.Models.LLMResponse
{
    internal class Choice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; } //indica la posicion de la respuesta en la lista
        [JsonPropertyName("message")]
        public ChoiceMessage Message { get; set; } = null!; // el contenido textual o la instrucciones 
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = null!; // indica porque el modelo terminó de genera rta

    }
}
