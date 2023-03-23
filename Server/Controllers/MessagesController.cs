using AutoMapper;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.BuildingBlocks;
using Server.Data;
using Server.Services;
using Shared.Messages;
using System;
using System.Linq;
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
        private readonly SkipCountProvider skipCountProvider;
        private readonly DataContextContainer dataContextContainer;
        public MessagesController(IMapper objectMapper, DataContextContainer dataContextContainer, MessageStreamingService messageStreamingService, SkipCountProvider skipCountProvider)
        {
            this.messageStreamingService = messageStreamingService;
            this.objectMapper = objectMapper;
            this.skipCountProvider = skipCountProvider;
            this.dataContextContainer = dataContextContainer;
        }

        [HttpGet]
        public async IAsyncEnumerable<MessageDTO> GetMessages()
        {
            await foreach(var message in messageStreamingService.ReadMessages(skipCountProvider.GetSkipCount()))
            {
                yield return objectMapper.Map<MessageDTO>(message);
            }
        }

        [HttpGet("relatedMessages/{messageId}")]
        public async Task<List<MessageDTO>> GetRelatedMessages([FromRoute] Guid messageId)
        {
            var message = dataContextContainer.Messages.SingleOrDefault(m => m.Id == messageId);
            if(message == null)
            {
                return null;
            }
            
            var relatedMessages = dataContextContainer.Messages.Skip(skipCountProvider.GetSkipCount() + new Random().Next(2500)).Take(new Random().Next(14)).ToList();

            var m = await messageStreamingService.ReadMessagesForAlliance(message.Alliance.AllianceId, skipCountProvider.GetSkipCount()).ToListAsync();

            return objectMapper.Map<List<MessageDTO>>(m);
        }
    }
}
