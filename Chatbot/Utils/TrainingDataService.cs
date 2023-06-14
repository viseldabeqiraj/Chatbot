using Chatbot.Dtos;
using Chatbot.Interfaces;
using Newtonsoft.Json;

namespace Chatbot.Utils
{
    public class TrainingDataService : ITrainingDataService
    {
        private const int BatchSize = 100;

        public async Task<List<IntentData>> LoadTrainingData()
        {
            List<IntentData> trainingData = new List<IntentData>();

            string jsonData = await File.ReadAllTextAsync("C:\\Users\\User\\Documents\\train.json");
            DatasetDto? dataset = JsonConvert.DeserializeObject<DatasetDto>(jsonData);

            foreach (ChatbotEntry entry in dataset.Entries)
            {
                foreach (QueryResult queryResult in entry.UsedQueries)
                {
                    string query = queryResult.Query;

                    foreach (Result result in queryResult.Results)
                    {
                        string message = result.Snippet;
                        string intent = result.Title;

                        trainingData.Add(new IntentData { Message = message, Intent = intent });

                        if (trainingData.Count >= BatchSize)
                        {
                            ProcessTrainingDataBatch(trainingData);
                        }
                    }
                }
            }

            // Process the remaining training data if it doesn't form a complete batch
            if (trainingData.Count > 0)
            {
                ProcessTrainingDataBatch(trainingData);
            }
            return trainingData;
        }

        private void ProcessTrainingDataBatch(List<IntentData> trainingData)
        {
            // Process the training data in the current batch (e.g., store it in a database, perform additional calculations, etc.)

            // Clear the training data list for the next batch
            trainingData.Clear();
        }

    }
}
