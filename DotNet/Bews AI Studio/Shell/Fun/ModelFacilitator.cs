using OpenAI.Chat;
using Azure;
using Azure.AI.OpenAI;

namespace Shell.Fun
{
    internal class ModelFacilitator(ModelData data)
    {
        private ChatClient ChatClient 
        {
            get
            {
                AzureOpenAIClient AzureOpenAIClient = new(data.BaseUri, new AzureKeyCredential(data.ApiKey));
                return AzureOpenAIClient.GetChatClient(data.DeploymentName);
            }
        }

        public Task<string> GetHaikuAsync(string crush1, string crush2, string relationship)
        {
            var requestOptions = new ChatCompletionOptions()
            {
                MaxOutputTokenCount = 4096,
                Temperature = 0.7f,
                TopP = 1.0f,
            };

            List<ChatMessage> messages =
            [
                new SystemChatMessage("You are an Expert haiku creator."),
                new SystemChatMessage("Make sure the relationship coming in the user chat message reflects in the Haiku"),
                new UserChatMessage($"{crush1} and {crush2} shares {relationship}. Get me a beautiful Haiku based on that."),
            ];

            return CompleteChatAsync(messages, requestOptions);
        }

        public Task<string> GetShortStory(string crush1, string crush2, string relationship)
        {
            var requestOptions = new ChatCompletionOptions()
            {
                MaxOutputTokenCount = 4096,
                Temperature = 1.0f,
                TopP = 1.0f,
            };

            List<ChatMessage> messages =
            [
                new SystemChatMessage("You are an Expert in creating short stories."),
                new SystemChatMessage("Make sure the relationship coming in the user chat message reflects in the Story"),
                new SystemChatMessage("Let not the story exceed, 500 chars"),
                new UserChatMessage($"{crush1} and {crush2} shares {relationship}. Get me a beautiful short story based on that."),
            ];

            return CompleteChatAsync(messages, requestOptions);
        }

        private async Task<string> CompleteChatAsync(List<ChatMessage> messages, ChatCompletionOptions options)
        {
            var response = await ChatClient.CompleteChatAsync(messages, options);
            return response.Value.Content[0].Text;
        }
    }
}