using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder;
using Chatbot.ChatbotHandler;
using Chatbot.Caching;
using Microsoft.Extensions.Caching.Memory;
using Chatbot.Interfaces;
using Chatbot.Indexing;
using Chatbot.MessageProcessing;

namespace Chatbot
{
    public static class DependencyInjection
    {
        public static void AddWeApiServices(this IServiceCollection services)
        {
            //services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton<IBotFrameworkHttpAdapter, BotFrameworkHttpAdapter>();
            services.AddTransient<IBot, ChatbotHandling>();
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IIndexService, IndexService>();
            services.AddSingleton<IMessageProcessor, MessageProcessor>();
            //services.AddSingleton<IMessageProcessor, MessageProcessor>();
        }
    }
}
