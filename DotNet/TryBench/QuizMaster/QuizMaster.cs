using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace QuizMaster
{
    public partial class QuizMaster : Form
    {
        OpenAIClient OpenAIClient = null!;

        public QuizMaster()
        {
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            LoadQuizDropdown();
            LoadQuizDifficultyLevel();
        }

        private void LoadQuizDropdown()
        {
            cmbQuizCategories.Items.AddRange(["Math", "Science", "AI & Technology", "General Knowledge", "Programming & Coding"]);
        }

        private void LoadQuizDifficultyLevel()
        {
            cmbQuizDifficultyLevel.Items.AddRange(["Beginner", "Intermeidate", "Expert"]);
        }

        private async void OnStartClickAsync(object sender, EventArgs e)
        {
            btnStart.Enabled = false;

            try
            {
                InitClient();
                await ChatClientBasedResponseAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Request Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnStart.Enabled = true;
            }
        }

        private async Task ChatClientBasedResponseAsync()
        {
            var QuizCategory = cmbQuizCategories.SelectedItem?.ToString() ?? "General Knowledge";
            var difficultyLevel = cmbQuizDifficultyLevel.SelectedItem?.ToString() ?? "Beginner";

            ChatClient chatClient = OpenAIClient.GetChatClient("gpt-4o");
            ChatCompletion response = await chatClient.CompleteChatAsync(QuizCategory);

            if (response != null)
            {
                //txtResponse.Text += GetResponse(response);
            }
        }


        private void InitClient()
        {
            string apiKey = Environment.GetEnvironmentVariable("GITHUB_TOKEN")
                ?? throw new InvalidOperationException("GITHUB_TOKEN environment variable is not set.");

            OpenAIClientOptions options = new() { Endpoint = new Uri("https://models.inference.ai.azure.com") };
            OpenAIClient = new OpenAIClient(new ApiKeyCredential(apiKey), options);
        }
    }
}