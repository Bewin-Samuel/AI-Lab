namespace Shell.Core.Providers;

internal sealed class HuggingFaceChatProvider(string apiKey, string model)
    : OpenAiCompatibleChatProvider(apiKey, model)
{
    protected override Uri Endpoint => new("https://api-inference.huggingface.co/v1/");
}
