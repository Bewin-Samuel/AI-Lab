namespace Shell.Core;

internal interface IChatCompletionProvider
{
    Task<string> CompleteChatAsync(string systemPrompt, string userMessage);
}
