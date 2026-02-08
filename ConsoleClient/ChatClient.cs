using LLM.OpenAI.Client.Models;
using LLM.OpenAI.Client.Services;
using Results;

namespace ConsoleClient
{
    internal class ChatClient(LlmClient llmClient)
    {
        public async Task StartAsync()
        {
            try
            {


                List<Message> context = [];
                context.Add(llmClient.CreateSystemMessage(
                    @"Eres un profesor de Matematicas. Limita la conversacion a temas relacionados con las matematicas. Responde de manera breve, clara y concisa.
                  "));

                string userPrompt;
                while (true)
                {
                    Console.Write("> ");
                    userPrompt = Console.ReadLine();

                    if (userPrompt == "salir")
                        break;
                    context.Add(llmClient.CreateUserMessage(userPrompt!));

                    Result response = await llmClient.ChatAsync(context);
                    Console.WriteLine();
                    if (response.IsSuccess)
                    {
                        Console.WriteLine($"<<< {context.Last().Content}");
                    }
                    else
                    {
                        Console.WriteLine(response.ErrorMessage);
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
