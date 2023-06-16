using Chatbot.ChatbotHandler;
using Chatbot.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;


namespace Chatbot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly BotAdapter _adapter;
        private readonly IBot _bot;
        private readonly IChatbotService _chatbotService;
        public ChatbotController(BotAdapter adapter, IBot bot, IChatbotService chatbotService)
        {
            _adapter = adapter;
            _bot = bot;
            _chatbotService = chatbotService;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveMessage()
        {
            var bot = new ChatbotHandling(_chatbotService);

            var httpContext = HttpContext;

            var body = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
            var receivedActivity = JsonConvert.DeserializeObject<Activity>(body);

            var turnContext = new TurnContext(_adapter, receivedActivity);

            await bot.OnTurnAsync(turnContext);

            return Ok();
        }

        [HttpPost(nameof(SendResponse))]
        public async Task<IActionResult> SendResponse()
        {
            var bot = new ChatbotHandling(_chatbotService);

            var httpContext = HttpContext;

            var body = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
            var receivedActivity = JsonConvert.DeserializeObject<Activity>(body);

            var turnContext = new TurnContext(_adapter, receivedActivity);

            await bot.OnTurnAsync(turnContext);

            return Ok();
        }

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
