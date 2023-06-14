using Chatbot.Dtos;

namespace Chatbot.Interfaces
{
    public interface IChatbotService
    {
        Task<List<Result>> GetResultsAsync(string userMessage);
        IntentData? ProcessUserMessage(string userMessage, List<Result> results);
    }
}
