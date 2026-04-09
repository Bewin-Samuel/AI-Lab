namespace Shell.Core.Providers;

internal sealed class GeminiChatProvider(string apiKey, string model)
    : OpenAiCompatibleChatProvider(apiKey, model)
{
    protected override Uri Endpoint => new("https://generativelanguage.googleapis.com/v1beta/openai/");
}
