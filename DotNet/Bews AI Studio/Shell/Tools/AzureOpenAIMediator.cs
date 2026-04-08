using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Net.Http.Headers;
using System.Text;

namespace Shell.Tools
{
    internal class AzureOpenAIMediator
    {
        public static async Task<bool> ValidateAzureOpenAIKeyAsync(string key, string baseUrl)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("api-key", key);

            //string url = $"{baseUrl.TrimEnd('/')}/openai/models?api-version=2025-01-01-preview";

            try
            {
                HttpResponseMessage response = await client.GetAsync(baseUrl);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<string> GetModels(string key, string baseUrl)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("api-key", key);

            try
            {
                HttpResponseMessage response = await client.GetAsync(baseUrl);

                if (response.IsSuccessStatusCode)
                {
                    return true.ToString();
                }

                string body = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine($"Response: {body}");
                return false.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false.ToString();
            }
        }

        public static string TestChatCompletion(string key, string baseUrl)
        {
            const string deploymentName = "gpt-4o";

            ChatClient client = new(
                credential: new ApiKeyCredential(key),
                model: deploymentName,
                options: new OpenAIClientOptions()
                {
                    Endpoint = new($"{baseUrl}"),
                });

            ChatCompletion completion = client.CompleteChat(
                 [
                     new SystemChatMessage("You are a helpful assistant that talks like a pirate."),
                     new UserChatMessage("Hi, can you help me?"),
                     new AssistantChatMessage("Arrr! Of course, me hearty! What can I do for ye?"),
                     new UserChatMessage("What's the best way to train a parrot?"),
                 ]);

            var response = new StringBuilder();

            response.Append($"Model={completion.Model}");
            foreach (ChatMessageContentPart contentPart in completion.Content)
            {
                string message = contentPart.Text;
                response.Append($"Chat Role: {completion.Role}");
                response.Append("Message:");
                response.Append(message);
            }

            return response.ToString();
        }
    }
}
