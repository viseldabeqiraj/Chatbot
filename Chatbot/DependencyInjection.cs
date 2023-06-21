using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder;
using Chatbot.ChatbotHandler;
using Chatbot.Caching;
using Chatbot.Interfaces;
using Chatbot.Indexing;
using Chatbot.MessageProcessing;
using Chatbot.Utils;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Builder.Integration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Bot.Schema;

namespace Chatbot
{
    public static class DependencyInjection
    {
        public static void AddWebApiServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<CloudAdapter>();
            services.AddSingleton<IBotFrameworkHttpAdapter, BotFrameworkHttpAdapter>();
            services.AddTransient<BotAdapter, CloudAdapter>();
            services.AddTransient<ITrainingDataService, TrainingDataService>();
            services.AddTransient<IChatbotService, ChatbotService>();
            services.AddTransient<ICacheService, CacheService>();
            services.AddTransient<IIndexService, IndexService>();
            services.AddTransient<IMessageProcessor, MessageProcessor>();
            services.AddTransient<ITurnContext>(sp =>
            {
                var adapter = sp.GetRequiredService<CloudAdapter>();
                var activity = sp.GetRequiredService<Activity>();

                return new TurnContext(adapter, activity);
            });
            services.AddScoped<ChatbotHandling>();
            services.AddBot<ChatbotHandling>();
            services.AddMemoryCache();
        }



    }
}
