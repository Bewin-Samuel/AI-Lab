using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace ExploreOpenAIApis
{
    public partial class ExploreOpenAIApis : Form
    {
        OpenAIClient OpenAIClient = null!;

        public ExploreOpenAIApis()
        {
            InitializeComponent();

            InitClient();
        }

        private void InitClient()
        {
            string apiKey = Environment.GetEnvironmentVariable("GITHUB_TOKEN")
                ?? throw new InvalidOperationException("GITHUB_TOKEN environment variable is not set.");

            OpenAIClientOptions options = new() { Endpoint = new Uri("https://models.inference.ai.azure.com") };
            OpenAIClient = new OpenAIClient(new ApiKeyCredential(apiKey), options);
        }

        private async void OnGoClickAsync(object sender, EventArgs e)
        {
            btnGo.Enabled = false;

            try
            {
                await ChatClientBasedResponseAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Request Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGo.Enabled = true;
            }
        }

        private async Task ChatClientBasedResponseAsync()
        {
            ChatClient chatClient = OpenAIClient.GetChatClient("gpt-4o");
            ChatCompletion response = await chatClient.CompleteChatAsync(txtPrompt.Text.Trim());

            if (response != null)
            {
                txtResponse.Text += GetResponse(response);
            }
        }

        private static string GetResponse(ChatCompletion response)
        {
            if (response.Content == null || response.Content.Count == 0)
            {
                return "No response received.";
            }

            return DateTime.Now.ToString("dd-MMM-yy hh:mm:ss") + " : " + response.Content[0].Text + Environment.NewLine + Environment.NewLine;
        }
    }
}