using System.Text;
using MsgReader.Outlook;
using UglyToad.PdfPig;

namespace Shell.Tools;

internal static class ContentReader
{
    private static readonly HashSet<string> SupportedExtensions = [".txt", ".pdf", ".msg"];

    public const string FileDialogFilter =
        "All supported files (*.txt;*.pdf;*.msg)|*.txt;*.pdf;*.msg" +
        "|Text files (*.txt)|*.txt" +
        "|PDF files (*.pdf)|*.pdf" +
        "|Outlook messages (*.msg)|*.msg";

    public static bool IsUrl(string input) =>
        Uri.TryCreate(input, UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

    public static async Task<string> ReadAsync(string input)
    {
        if (IsUrl(input))
            return await ReadFromUrlAsync(input);

        return await ReadFromFileAsync(input);
    }

    private static async Task<string> ReadFromUrlAsync(string url)
    {
        using var httpClient = new HttpClient();
        return await httpClient.GetStringAsync(url);
    }

    private static Task<string> ReadFromFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        var extension = Path.GetExtension(filePath).ToLowerInvariant();

        if (!SupportedExtensions.Contains(extension))
            throw new NotSupportedException(
                $"File type '{extension}' is not supported. Supported types: {string.Join(", ", SupportedExtensions)}");

        return extension switch
        {
            ".txt" => File.ReadAllTextAsync(filePath),
            ".pdf" => Task.FromResult(ReadPdf(filePath)),
            ".msg" => Task.FromResult(ReadOutlookMessage(filePath)),
            _ => throw new NotSupportedException($"File type '{extension}' is not supported.")
        };
    }

    private static string ReadPdf(string filePath)
    {
        using var document = PdfDocument.Open(filePath);
        var textBuilder = new StringBuilder();

        foreach (var page in document.GetPages())
            textBuilder.AppendLine(page.Text);

        return textBuilder.ToString().Trim();
    }

    private static string ReadOutlookMessage(string filePath)
    {
        using var message = new Storage.Message(filePath);
        var builder = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(message.Subject))
            builder.AppendLine($"Subject: {message.Subject}");

        if (!string.IsNullOrWhiteSpace(message.Sender?.Email))
            builder.AppendLine($"From: {message.Sender.Email}");

        if (message.SentOn.HasValue)
            builder.AppendLine($"Date: {message.SentOn.Value}");

        builder.AppendLine();
        builder.AppendLine(message.BodyText ?? message.BodyHtml ?? string.Empty);

        return builder.ToString().Trim();
    }
}
