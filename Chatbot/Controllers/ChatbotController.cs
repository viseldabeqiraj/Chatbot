using Chatbot.ChatbotHandler;
using Chatbot.Dtos;
using Chatbot.Interfaces;
using Microsoft.AspNetCore.Http;
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
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly IBot _bot;
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IBotFrameworkHttpAdapter adapter, IBot bot, IChatbotService chatbotService)
        {
            _adapter = adapter;
            _bot = bot;
            _chatbotService = chatbotService;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveMessage([FromBody] Activity activity)
        {
            var bot = new ChatbotHandling(_chatbotService); // Injected instance of IChatbotService

            var httpContext = HttpContext;

            await _adapter.ProcessAsync(httpContext.Request, httpContext.Response, bot, CancellationToken.None);

            return Ok();
        }
        [HttpPost(nameof(SendResponse))]
        public async Task<IActionResult> SendResponse()
        {
            // Read the incoming request
            var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

            // Convert the request body to an Activity
            var activity = JsonConvert.DeserializeObject<Activity>(requestBody);

            // Create a turn context from the incoming activity
            var turnContext = new TurnContext(_adapter, activity);

            // Process the turn context
            await _bot.OnTurnAsync(turnContext);

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
