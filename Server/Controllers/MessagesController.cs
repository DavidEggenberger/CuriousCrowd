using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessageStreamingService messageStreamingService;
        public MessagesController(MessageStreamingService messageStreamingService)
        {
            this.messageStreamingService = messageStreamingService;
        }

        [HttpGet]
        public async Task<ActionResult> GetMessages()
        {
            return Ok(messageStreamingService.ReadMessages());
        }
    }
}
