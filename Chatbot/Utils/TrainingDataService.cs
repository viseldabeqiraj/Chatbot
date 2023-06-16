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
            var dataset = JsonConvert.DeserializeObject<List<DatasetDto>>(jsonData);

            foreach (var data in dataset)
            {
                foreach (var queryResult in data.ViewedDocTitles)
                {
                    foreach (var result in data.UsedQueries[0].Results)
                    {
                        string message = result.Snippet;
                        string intent = result.Title;

                        trainingData.Add(new IntentData { Message = message, Intent = intent });

                        if (trainingData.Count >= BatchSize)
                        {
                            //ProcessTrainingDataBatch(trainingData);
                            //trainingData.Clear();
                        }
                    }
                }
            }

            // Process the remaining training data if it doesn't form a complete batch
            //if (trainingData.Count > 0)
            //{
            //    ProcessTrainingDataBatch(trainingData);
            //}

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
