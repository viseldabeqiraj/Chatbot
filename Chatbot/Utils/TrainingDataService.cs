using Chatbot.Dtos;
using Chatbot.Helpers;
using Chatbot.Interfaces;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Chatbot.Utils
{
    public class TrainingDataService : ITrainingDataService
    {
        private const int BatchSize = 100;

        public async Task<List<IntentData>> LoadTrainingData()
        {
            var trainingData = new ConcurrentBag<IntentData>();

            using (var fileStream = File.OpenRead("C:\\Users\\User\\Documents\\train.json"))
            using (var jsonDocument = await JsonDocument.ParseAsync(fileStream))
            {
                var tasks = new List<Task>();

                foreach (var jsonElement in jsonDocument.RootElement.EnumerateArray())
                {
                    var test = jsonElement.GetRawText();
                    var dataset = await JsonSerializer.DeserializeAsync<List<DatasetDto>>(StreamExtensions.ToStream(jsonElement.GetRawText()));
                    tasks.Add(ProcessDatasetAsync(dataset, trainingData));
                }

                await Task.WhenAll(tasks);
            }

            return trainingData.ToList();
        }


        private async Task ProcessDatasetAsync(List<DatasetDto> dataset, ConcurrentBag<IntentData> trainingData)
        {
            Parallel.ForEach(dataset, async data =>
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
                            await ProcessTrainingDataBatchAsync(trainingData);
                        }
                    }
                }
            });
        }

        private async Task ProcessTrainingDataBatchAsync(ConcurrentBag<IntentData> trainingData)
        {
            // Process the training data in the current batch
            await Task.Delay(100); // Simulated processing time

            // Clear the training data in the current batch
            trainingData.Clear();
        }



        private async Task ProcessTrainingDataBatchAsync(List<IntentData> trainingData)
        {
            // Process the training data in the current batch (e.g., store it in a database, perform additional calculations, etc.)
            // This method can be made asynchronous as well if the processing steps are async.

            await Task.Delay(100); // Simulated processing time

            // Clear the training data list for the next batch
            //trainingData.Clear();
        }
    }
}
