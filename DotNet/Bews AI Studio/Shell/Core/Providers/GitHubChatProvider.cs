namespace Shell.Core.Providers;

internal sealed class GitHubChatProvider(string apiKey, string model)
    : OpenAiCompatibleChatProvider(apiKey, model)
{
    protected override Uri Endpoint => new("https://models.inference.ai.azure.com");
}
