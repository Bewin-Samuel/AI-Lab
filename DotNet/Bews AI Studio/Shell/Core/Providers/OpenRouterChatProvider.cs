namespace Shell.Core.Providers;

internal sealed class OpenRouterChatProvider(string apiKey, string model)
    : OpenAiCompatibleChatProvider(apiKey, model)
{
    protected override Uri Endpoint => new("https://openrouter.ai/api/v1");
}
