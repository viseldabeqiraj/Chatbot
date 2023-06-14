using Chatbot.Dtos;

namespace Chatbot.Interfaces
{
    public interface ITrainingDataService
    {
        Task<List<IntentData>> LoadTrainingData();
    }
}
