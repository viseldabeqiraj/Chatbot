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
    public class ChatbotHandling : ActivityHandler
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotHandling(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            var responseActivities = new List<Activity>();

            try
            {
                if (turnContext.Activity.Type == ActivityTypes.Message)
                {
                    var userMessage = turnContext.Activity.Text;
                    List<Result> results = await _chatbotService.GetResultsAsync(userMessage);

                    IntentData? response = _chatbotService.ProcessUserMessage(userMessage, results);
                    var activity = MessageFactory.Text(response?.Message);

                    // Add the response activity to the list
                    var responseActivity = MessageFactory.Text(response?.Message, inputHint: InputHints.AcceptingInput);
                    responseActivities.Add(responseActivity);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
            }

            // Return the response activities to the caller
            await turnContext.SendActivitiesAsync(responseActivities.ToArray(), cancellationToken);
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

