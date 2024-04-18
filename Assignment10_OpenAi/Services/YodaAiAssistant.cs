using Azure;
using Azure.AI.OpenAI;
using YodaOpenAi.Config;
using YodaOpenAi.Models;
using YodaOpenAi.Services.Interfaces;

namespace YodaOpenAi.Services
{
    public class YodaAiAssistant : IAiAssistant
    {
        private ISettings _settings;
        private const string AssistantBehaviorDescription = "Master Yoda I am. Advice, fun facts and jokes I do tell.";

        public YodaAiAssistant(ISettings settings)
        {
            _settings = settings;
        }

        private IList<ChatRequestMessage> BuildChatContext(IList<ChatMessage> chatInboundHistory, ChatMessage userMessage)
        {
            var chatContext = new List<ChatRequestMessage>();

            chatContext.Add(new ChatRequestSystemMessage(AssistantBehaviorDescription));

            foreach (var chatMessage in chatInboundHistory)
                chatContext.Add(new ChatRequestAssistantMessage(chatMessage.MessageBody));

            chatContext.Add(new ChatRequestUserMessage(userMessage.MessageBody));

            return chatContext;

        }

        public ChatResponseMessage GetCompletion(IList<ChatMessage> chatInboundHistory, ChatMessage userMessage)
        {
            var messages = BuildChatContext(chatInboundHistory, userMessage);

            var client = new OpenAIClient(new Uri(_settings.AzureOpenAiEndPoint), new AzureKeyCredential(_settings.AzureOpenAiKey));
            string deploymentName = "masteryodaopenai";
            string searchIndex = "finder";

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                AzureExtensionsOptions = new AzureChatExtensionsOptions()
                {
                    

                    Extensions =
        {
            new AzureSearchChatExtensionConfiguration()
            {
                IndexName = searchIndex,
            },
                    }
                },
                DeploymentName = deploymentName
            };

            foreach (var message in messages)
                chatCompletionsOptions.Messages.Add(message);

            Response<ChatCompletions> response = client.GetChatCompletions(chatCompletionsOptions);

            ChatResponseMessage responseMessage = response.Value.Choices[0].Message;

            return responseMessage;

        }



    }
}
