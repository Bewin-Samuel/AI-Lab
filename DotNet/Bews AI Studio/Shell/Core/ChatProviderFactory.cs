using Shell.Core.Providers;

namespace Shell.Core;

internal static class ChatProviderFactory
{
    public static IChatCompletionProvider Create(string provider, string apiKey, string model, string baseUrl = "")
    {
        return provider switch
        {
            "Azure Open AI" => new AzureOpenAIChatProvider(apiKey, model, baseUrl),
            "Open AI"       => new OpenAIChatProvider(apiKey, model),
            "Open Router"   => new OpenRouterChatProvider(apiKey, model),
            "GitHub"        => new GitHubChatProvider(apiKey, model),
            "Gemini"        => new GeminiChatProvider(apiKey, model),
            "Hugging Face"  => new HuggingFaceChatProvider(apiKey, model),
            _               => throw new NotSupportedException($"Provider '{provider}' is not supported.")
        };
    }
}
