using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using Shell.Tools;

namespace Shell.Core
{
    internal sealed record ModelInfo(string Id, string DisplayName, string ModelType, string Params);

    internal sealed class ModelProvider
    {
        private static readonly HttpClient HttpClient = new();

        public async Task<bool> ValidateKeyAsync(string provider, string apiKey, string baseUrl = "")
        {
            var key = apiKey.Trim();
            if (string.IsNullOrWhiteSpace(key))
                return false;

            return provider switch
            {
                "Azure Open AI" => await AzureOpenAIMediator.ValidateAzureOpenAIKeyAsync(key, baseUrl),
                "Open Router"  => await ValidateWithBearerGetAsync("https://openrouter.ai/api/v1/auth/key", key),
                "Open AI"      => await ValidateWithBearerGetAsync("https://api.openai.com/v1/models", key),
                "Hugging Face" => await ValidateWithBearerGetAsync("https://huggingface.co/api/whoami-v2", key),
                "Gemini"       => await ValidateGeminiKeyAsync(key),
                "GitHub"       => await ValidateGitHubKeyAsync(key),
                _              => false
            };
        }

        public async Task<IReadOnlyList<ModelInfo>> GetModelsAsync(string provider, string apiKey, string baseUrl = "")
        {
            var key = apiKey.Trim();
            if (string.IsNullOrWhiteSpace(key))
                return [];

            return provider switch
            {
                "Open AI"      => await GetOpenAiModelsAsync(key),
                "Open Router"  => await GetOpenRouterModelsAsync(key),
                "Gemini"       => await GetGeminiModelsAsync(key),
                "Hugging Face" => await GetHuggingFaceModelsAsync(key),
                "GitHub"       => await GetGitHubModelsAsync(key),
                _              => []
            };
        }

        private static async Task<bool> ValidateWithBearerGetAsync(string url, string key)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
                using var response = await HttpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        private static async Task<bool> ValidateGeminiKeyAsync(string key)
        {
            try
            {
                var uri = $"https://generativelanguage.googleapis.com/v1beta/models?key={Uri.EscapeDataString(key)}";
                using var request = new HttpRequestMessage(HttpMethod.Get, uri);
                using var response = await HttpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        private static async Task<bool> ValidateGitHubKeyAsync(string key)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
                request.Headers.UserAgent.ParseAdd("BewsAIStudio/1.0");
                request.Headers.Accept.ParseAdd("application/vnd.github+json");
                using var response = await HttpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        private static async Task<IReadOnlyList<ModelInfo>> GetOpenAiModelsAsync(string key)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.openai.com/v1/models");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
            using var response = await HttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return [];

            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await JsonDocument.ParseAsync(stream);

            if (!document.RootElement.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
                return [];

            return [.. data.EnumerateArray().Select(model =>
            {
                var id = GetStringProperty(model, "id");
                return new ModelInfo(id, id, "N/A", ExtractParams(id));
            })];
        }

        private static async Task<IReadOnlyList<ModelInfo>> GetOpenRouterModelsAsync(string key)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://openrouter.ai/api/v1/models");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
            using var response = await HttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return [];

            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await JsonDocument.ParseAsync(stream);

            if (!document.RootElement.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
                return [];

            return [.. data.EnumerateArray().Select(model =>
            {
                var id   = GetStringProperty(model, "id");
                var name = GetStringProperty(model, "name", id);
                var modelType = "Paid";

                if (model.TryGetProperty("pricing", out var pricing))
                {
                    var prompt     = GetDecimalProperty(pricing, "prompt");
                    var completion = GetDecimalProperty(pricing, "completion");
                    modelType = prompt.GetValueOrDefault() <= 0m && completion.GetValueOrDefault() <= 0m ? "Free" : "Paid";
                }

                return new ModelInfo(id, name, modelType, ExtractParams(id, name));
            })];
        }

        private static async Task<IReadOnlyList<ModelInfo>> GetGeminiModelsAsync(string key)
        {
            var uri = $"https://generativelanguage.googleapis.com/v1beta/models?key={Uri.EscapeDataString(key)}";
            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            using var response = await HttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return [];

            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await JsonDocument.ParseAsync(stream);

            if (!document.RootElement.TryGetProperty("models", out var models) || models.ValueKind != JsonValueKind.Array)
                return [];

            return [.. models.EnumerateArray().Select(model =>
            {
                var id   = GetStringProperty(model, "name");
                var name = GetStringProperty(model, "displayName", id);
                return new ModelInfo(id, name, "N/A", ExtractParams(id, name));
            })];
        }

        private static async Task<IReadOnlyList<ModelInfo>> GetHuggingFaceModelsAsync(string key)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://huggingface.co/api/models?limit=100");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
            using var response = await HttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return [];

            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await JsonDocument.ParseAsync(stream);

            if (document.RootElement.ValueKind != JsonValueKind.Array)
                return [];

            return [.. document.RootElement.EnumerateArray().Select(model =>
            {
                var id       = GetStringProperty(model, "id");
                var hfParams = TryGetHuggingFaceParams(model);
                return new ModelInfo(id, id, "N/A", hfParams ?? ExtractParams(id));
            })];
        }

        private static async Task<IReadOnlyList<ModelInfo>> GetGitHubModelsAsync(string key)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://models.inference.ai.azure.com/models");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
            request.Headers.UserAgent.ParseAdd("BewsAIStudio/1.0");
            using var response = await HttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return [];

            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await JsonDocument.ParseAsync(stream);

            if (document.RootElement.ValueKind != JsonValueKind.Array)
                return [];

            return [.. document.RootElement.EnumerateArray().Select(model =>
            {
                var id   = GetStringProperty(model, "id");
                var name = GetStringProperty(model, "name", id);
                return new ModelInfo(id, name, "N/A", ExtractParams(id, name));
            })];
        }

        private static string GetStringProperty(JsonElement element, string name, string fallback = "")
            => element.TryGetProperty(name, out var prop) ? prop.GetString() ?? fallback : fallback;

        private static decimal? GetDecimalProperty(JsonElement element, string name)
        {
            if (!element.TryGetProperty(name, out var value))
                return null;

            if (value.ValueKind == JsonValueKind.Number && value.TryGetDecimal(out var num))
                return num;

            if (value.ValueKind == JsonValueKind.String && decimal.TryParse(value.GetString(), out var str))
                return str;

            return null;
        }

        private static string? TryGetHuggingFaceParams(JsonElement model)
        {
            if (!model.TryGetProperty("safetensors", out var safetensors) || safetensors.ValueKind != JsonValueKind.Object)
                return null;

            if (!safetensors.TryGetProperty("total", out var total) ||
                total.ValueKind != JsonValueKind.Number ||
                !total.TryGetInt64(out var totalParams) || totalParams <= 0)
                return null;

            return FormatParamCount(totalParams);
        }

        private static string FormatParamCount(long count) => count switch
        {
            >= 1_000_000_000 => $"{count / 1_000_000_000d:0.##}B",
            >= 1_000_000     => $"{count / 1_000_000d:0.##}M",
            >= 1_000         => $"{count / 1_000d:0.##}K",
            _                => count.ToString()
        };

        private static string ExtractParams(params string[] names)
        {
            var regex = new Regex(@"(\d+\.?\d*)[xX]?(\d+\.?\d*)?[\s\-_]?[bB]", RegexOptions.IgnoreCase);
            foreach (var name in names)
            {
                if (string.IsNullOrWhiteSpace(name))
                    continue;
                var match = regex.Match(name);
                if (match.Success)
                    return match.Value.Trim('-', '_', ' ').ToUpperInvariant();
            }
            return "N/A";
        }
    }
}
