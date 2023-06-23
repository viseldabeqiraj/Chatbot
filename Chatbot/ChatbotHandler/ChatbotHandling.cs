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
        private Activity? _responseActivity; 

        public ChatbotHandling(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }
        public Activity GetResponseActivity()
        {
            return _responseActivity;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            try
            {
                if (turnContext.Activity.Type == ActivityTypes.Message)
                {
                    var userMessage = turnContext.Activity.Text;
                    List<Result> results = await _chatbotService.GetResultsAsync(userMessage);

                    IntentData? response = _chatbotService.ProcessUserMessage(userMessage, results);

                    // Create the response activity
                    var responseActivity = MessageFactory.Text(response?.Message);
                    responseActivity.Recipient = turnContext.Activity.From;
                    responseActivity.ChannelId = turnContext.Activity.ChannelId;
                    responseActivity.Type = ActivityTypesEx.InvokeResponse;
                    _responseActivity = responseActivity;



                    await turnContext.SendActivityAsync(responseActivity, cancellationToken).ConfigureAwait(false);
                   // await turnContext.SendActivityAsync(new Activity { Value = response, Type = ActivityTypesEx.InvokeResponse }, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                // Log the exception for troubleshooting
                Console.WriteLine($"An error occurred: {ex}");

                // Return an error response if desired
                var errorMessage = MessageFactory.Text("An error occurred while processing your request.");
                await turnContext.SendActivityAsync(errorMessage, cancellationToken);
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

