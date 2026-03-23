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
            var GITHUB_TOKEN = "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";
            //string apiKey = Environment.GetEnvironmentVariable("GITHUB_TOKEN")
            //    ?? throw new InvalidOperationException("GITHUB_TOKEN environment variable is not set.");

            OpenAIClientOptions options = new() { Endpoint = new Uri("https://models.inference.ai.azure.com") };
            OpenAIClient = new OpenAIClient(new ApiKeyCredential(GITHUB_TOKEN), options);
        }

        private async void OnGoClickAsync(object sender, EventArgs e)
        {
            ChatClientBasedResponse();
        }

        private async void ChatClientBasedResponse()
        {
            ChatClient chatClient = OpenAIClient.GetChatClient("gpt-4o");
            ChatCompletion response = await chatClient.CompleteChatAsync(txtPrompt.Text.Trim());

            if (response != null)
            {
                txtResponse.Text += GetResponse(response);
            }
        }

        //private async void RecommendedResponseBasedClient()
        //{
        //    var chatClient = OpenAIClient.CreateResponseClient("gpt-4o", ResponseType.ChatCompletion);
        //    ChatCompletion response = await chatClient.CompleteChatAsync(txtPrompt.Text.Trim());

        //    if (response != null)
        //    {
        //        txtResponse.Text += GetResponse(response);
        //    }
        //}

        private string GetResponse(ChatCompletion response)
        {
            if (response == null || response.Content == null || response.Content.Count == 0)
            {
                return "No response received.";
            }

            return DateTime.Now.ToString("dd-MMM-yy hh:mm:ss") + " : " + response.Content[0].Text + Environment.NewLine + Environment.NewLine;
        }
    }
}