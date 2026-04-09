using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace Shell.Core.Providers;

internal abstract class OpenAiCompatibleChatProvider(string apiKey, string model) : IChatCompletionProvider
{
    protected virtual Uri? Endpoint => null;

    public async Task<string> CompleteChatAsync(string systemPrompt, string userMessage)
    {
        var credential = new ApiKeyCredential(apiKey);

        var client = Endpoint is { } endpoint
            ? new ChatClient(model, credential, new OpenAIClientOptions { Endpoint = endpoint })
            : new ChatClient(model, credential);

        List<ChatMessage> messages =
        [
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userMessage)
        ];

        var response = await client.CompleteChatAsync(messages);
        return response.Value.Content[0].Text;
    }
}
