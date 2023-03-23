using AutoMapper;
using Server.Data;
using Shared.Messages;

namespace Server.BuildingBlocks.Mappings
{
    public class AllianceMapping : Profile
    {
        public AllianceMapping()
        {
            CreateMap<Alliance, AllianceDTO>();
        }
    }
}
