using AutoMapper;
using Server.Data;
using Shared.Messages;

namespace Server.BuildingBlocks.Mappings
{
    public class MessageMapping : Profile
    {
        public MessageMapping()
        {
            CreateMap<Message, MessageDTO>();
        }
    }
}
