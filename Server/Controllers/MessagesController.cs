using AutoMapper;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessageStreamingService messageStreamingService;
        private readonly IMapper objectMapper;
        public MessagesController(IMapper objectMapper, MessageStreamingService messageStreamingService)
        {
            this.messageStreamingService = messageStreamingService;
            this.objectMapper = objectMapper;
        }

        [HttpGet]
        public async IAsyncEnumerable<MessageDTO> GetMessages()
        {
            await foreach(var message in messageStreamingService.ReadMessages())
            {
                yield return objectMapper.Map<MessageDTO>(message);
            }
        }
    }
}
