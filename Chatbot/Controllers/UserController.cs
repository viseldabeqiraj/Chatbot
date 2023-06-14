using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController()
        {
            
        }
        [HttpPost(nameof(RecieveMessage))]
        public async Task<IActionResult> RecieveMessage()
        {
            return Ok();
        }
    }
}
