using Chatbot.Dtos;

namespace Chatbot.Interfaces
{
    public interface IIndexService
    {
        string NormalizeMessage(string message);
        void BuildIndex(List<IntentData> trainingData, Dictionary<string, List<Result>> index);
    }
}
