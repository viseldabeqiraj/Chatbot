using Chatbot.Dtos;
using Chatbot.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace Chatbot.ChatbotHandler
{
    /// <summary>
    /// A class that implements the IBot interface or inherits from a bot framework-specific base class (e.g., ActivityHandler). 
    /// This is where you'll define the chatbot's behavior and handle user messages.
    /// </summary>
    public class ChatbotHandling : IBot //ActivityHandler
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotHandling(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            try
            {
                if (turnContext.Activity.Type == ActivityTypes.Message)
                {
                    var userMessage = turnContext.Activity.Text;
                    turnContext.Activity.Locale = "en-US"; 
                    List<Result> results = await _chatbotService.GetResultsAsync(userMessage);

                    IntentData? response = _chatbotService.ProcessUserMessage(userMessage, results);
                    var test = MessageFactory.Text(response?.Message);
                    await turnContext.SendActivityAsync(MessageFactory.Text(response?.Message), cancellationToken);
                }
            }
            catch(Exception ex) 
            { 

            }
        }

        /// <summary>
        /// method is overridden to handle incoming message activities from the user. 
        /// Inside this method, you can extract the user message, process it using chatbot's logic, and generate a response.
        /// </summary>
        /// <param name="turnContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        
        //protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        //{
        //    var userMessage = turnContext.Activity.Text;

        //    List<Result> results = await _chatbotService.GetResultsAsync(userMessage);

        //    IntentData? response = _chatbotService.ProcessUserMessage(userMessage, results);

        //    await turnContext.SendActivityAsync(MessageFactory.Text(response.Message), cancellationToken);
        //}
    }

}

