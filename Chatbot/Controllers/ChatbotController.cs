using Chatbot.ChatbotHandler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;


namespace Chatbot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly BotAdapter _adapter;
        private readonly ChatbotHandling _chatbotHandling;
        public ChatbotController(BotAdapter adapter, ChatbotHandling chatbotHandling)
        {
            _adapter = adapter;
            _chatbotHandling = chatbotHandling;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveMessage([FromBody] Activity activity)
        {
            var turnContext = new TurnContext(_adapter, activity);
            var responseActivity = default(Activity);

            try
            {
                turnContext.OnSendActivities(async (ctx, activities, next) =>
                {
                    responseActivity = activities.FirstOrDefault();
                    return await next().ConfigureAwait(false);
                });

                var cancellationToken = default(CancellationToken);
                await _chatbotHandling.OnTurnAsync(turnContext, cancellationToken);

                if (responseActivity != null)
                {
                    return Ok(responseActivity);
                }
                else
                {
                    // No response activity found
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return StatusCode(500); // or any appropriate error response
            }
        }






        //[HttpPost(nameof(SendResponse))]
        //public async Task<IActionResult> SendResponse()
        //{
        //    var bot = new ChatbotHandling(_chatbotService);

        //    var httpContext = HttpContext;

        //    var body = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
        //    var receivedActivity = JsonConvert.DeserializeObject<Activity>(body);

        //    var turnContext = new TurnContext(_adapter, receivedActivity);

        //    await bot.OnTurnAsync(turnContext);

        //    return Ok();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Chat(string userMessage)
        //{
        //    try
        //    {
        //        List<Result> results = await _chatbotService.GetResultsAsync(userMessage);
        //        IntentData? intentData = _chatbotService.ProcessUserMessage(userMessage, results);

        //        // Process the intentData and prepare the response

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions that occur during processing
        //        return StatusCode(500, "An error occurred: " + ex.Message);
        //    }
        //}

    }
}
