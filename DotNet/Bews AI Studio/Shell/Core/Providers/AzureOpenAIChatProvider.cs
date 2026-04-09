using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace Shell.Core.Providers;

internal sealed class AzureOpenAIChatProvider(string apiKey, string model, string baseUrl) : IChatCompletionProvider
{
    public async Task<string> CompleteChatAsync(string systemPrompt, string userMessage)
    {
        var azureClient = new AzureOpenAIClient(new Uri(baseUrl), new AzureKeyCredential(apiKey));
        var chatClient = azureClient.GetChatClient(model);

        List<ChatMessage> messages =
        [
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userMessage)
        ];

        var response = await chatClient.CompleteChatAsync(messages);
        return response.Value.Content[0].Text;
    }
}
