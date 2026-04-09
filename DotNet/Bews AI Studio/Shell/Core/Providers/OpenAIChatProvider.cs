namespace Shell.Core.Providers;

internal sealed class OpenAIChatProvider(string apiKey, string model)
    : OpenAiCompatibleChatProvider(apiKey, model);
