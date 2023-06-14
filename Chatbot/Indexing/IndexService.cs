using Chatbot.ChatbotHandler;
using Chatbot.Dtos;
using Chatbot.Interfaces;
using System;

namespace Chatbot.Indexing
{
    public class IndexService : IIndexService
    {
        public void BuildIndex(List<IntentData> trainingData, Dictionary<string, List<Result>> index)
        {
            foreach (IntentData data in trainingData)
            {
                string normalizedMessage = NormalizeMessage(data.Message);

                if (!index.ContainsKey(normalizedMessage))
                {
                    index[normalizedMessage] = new List<Result>();
                }

                index[normalizedMessage].Add(new Result { Title = data.Intent, Snippet = data.Message });
            }
        }

        public string NormalizeMessage(string message)
        {
            return message.ToLowerInvariant();
        }
    }
}
