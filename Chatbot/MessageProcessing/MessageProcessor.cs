using Chatbot.Dtos;
using Chatbot.Interfaces;
using MinimumEditDistance;

namespace Chatbot.MessageProcessing
{
    public class MessageProcessor : IMessageProcessor
    {
        public IntentData ProcessUserMessage(string message, List<Result> trainingData)
        {
            if (trainingData.Count == 0)
                return new IntentData { Message = "I do not have information about that.", Intent = "Error" };

            Result? bestMatch = null;
            int minDistance = int.MaxValue;

            foreach (var data in trainingData)
            {
                int distance = Levenshtein.CalculateDistance(message, data.Snippet, 1);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestMatch = data;
                }
            }

            return new IntentData { Message = bestMatch.Snippet, Intent = bestMatch.Title };
        }
    }
}
