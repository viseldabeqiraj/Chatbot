using Chatbot.Dtos;
using Chatbot.Interfaces;

namespace Chatbot.ChatbotHandler
{
    public class ChatbotService : IChatbotService
    {
        private readonly ICacheService _cacheService;
        private readonly IIndexService _indexService;
        private readonly IMessageProcessor _messageProcessor;
        private readonly ITrainingDataService _trainingDataService;
        private Dictionary<string, List<Result>> _index;
        private List<IntentData> _trainingData;

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

        private Task<List<Result>> FetchResultsFromTrainingDataAsync(string userMessage)
        {
            List<Result> results = _trainingData
                .Where(data => string.Equals(_indexService.NormalizeMessage(data.Intent), _indexService.NormalizeMessage(userMessage), StringComparison.OrdinalIgnoreCase))
                .Select(data => new Result { Title = data.Intent, Snippet = data.Message })
                .ToList();

            return Task.FromResult(results);
        }

        private async Task BuildIndexAsync()
        {
            _trainingData = await _trainingDataService.LoadTrainingData().ConfigureAwait(false);
            _indexService.BuildIndex(_trainingData, _index);
        }

        public IntentData? ProcessUserMessage(string userMessage, List<Result> results)
        {
            return _messageProcessor.ProcessUserMessage(userMessage, results);
        }
    }
}
