using Azure.AI.OpenAI;
using YodaOpenAi.Models;

namespace YodaOpenAi.Services.Interfaces
{
    public interface IAiAssistant
    {
        ChatResponseMessage GetCompletion(IList<ChatMessage> chatInboundHistory, ChatMessage userMessage);
    }
}
