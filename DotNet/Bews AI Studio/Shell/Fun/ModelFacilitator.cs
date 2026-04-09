using OpenAI.Chat;
using Azure;
using Azure.AI.OpenAI;

namespace Shell.Fun
{
    internal class ModelFacilitator(ModelData data)
    {
        public AzureOpenAIClient AzureOpenAIClient => new AzureOpenAIClient(data.BaseUri, new AzureKeyCredential(data.ApiKey));

        public async Task<string> GetHaikuAsync(string crush1, string crush2, string relationship)
        {
            ChatClient chatClient = AzureOpenAIClient.GetChatClient(data.DeploymentName);

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

            var response = await chatClient.CompleteChatAsync(messages, requestOptions);

            return response.Value.Content[0].Text;
        }

        public async Task<string> GetShortStory(string crush1, string crush2, string relationship)
        {
            ChatClient chatClient = AzureOpenAIClient.GetChatClient(data.DeploymentName);

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

            var response = await chatClient.CompleteChatAsync(messages, requestOptions);

            return response.Value.Content[0].Text;
        }
    }
}