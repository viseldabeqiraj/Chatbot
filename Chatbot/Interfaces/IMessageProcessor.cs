using Chatbot.Dtos;

namespace Chatbot.Interfaces
{
    public interface IMessageProcessor
    {
        IntentData ProcessUserMessage(string message, List<Result> trainingData);
    }
}
