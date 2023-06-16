using Chatbot.Dtos;
using Chatbot.Interfaces;
using Chatbot.Utils;

namespace Chatbot.ChatbotHandler
{
    public class ChatbotService : IChatbotService
    {
        private readonly ICacheService _cacheService;
        private readonly IIndexService _indexService;
        private readonly IMessageProcessor _messageProcessor;
        private readonly ITrainingDataService _trainingDataService;
        private Dictionary<string, List<Result>> _index;

        public ChatbotService(ICacheService cacheService, IIndexService indexService, IMessageProcessor messageProcessor, ITrainingDataService trainingDataService)
        {
            _cacheService = cacheService;
            _indexService = indexService;
            _messageProcessor = messageProcessor;
            _trainingDataService = trainingDataService;
            _index = new Dictionary<string, List<Result>>();
            BuildIndexAsync().Wait();
        }

        public async Task<List<Result>> GetResultsAsync(string userMessage)
        {
            if (_cacheService.TryGetItem(userMessage, out List<Result> results))
            {
                return results;
            }

            string normalizedMessage = _indexService.NormalizeMessage(userMessage);
            if (_index.TryGetValue(normalizedMessage, out List<Result>? indexedResults))
            {
                results = indexedResults;
            }
            else
            {
                results = await FetchResultsFromTrainingDataAsync(userMessage);
            }

            _cacheService.AddItem(userMessage, results);

            return results;
        }

        private async Task<List<Result>> FetchResultsFromTrainingDataAsync(string userMessage)
        {
            List<IntentData> trainingData = await FetchTrainingDataAsync().ConfigureAwait(false);
            List<Result> results = trainingData
                .Where(data => string.Equals(_indexService.NormalizeMessage(data.Intent), _indexService.NormalizeMessage(userMessage), StringComparison.OrdinalIgnoreCase))
                .Select(data => new Result { Title = data.Intent, Snippet = data.Message })
                .ToList();

            return results;
        }

        private async Task<List<IntentData>> FetchTrainingDataAsync()
        {
            List<IntentData> trainingData = await _trainingDataService.LoadTrainingData().ConfigureAwait(false);
            return trainingData;

        }

        public IntentData? ProcessUserMessage(string userMessage, List<Result> results)
        {
            return _messageProcessor.ProcessUserMessage(userMessage, results);
        }

        private async Task BuildIndexAsync()
        {
            List<IntentData> trainingData = await FetchTrainingDataAsync();
            _indexService.BuildIndex(trainingData, _index);
        }
    
    }
}
