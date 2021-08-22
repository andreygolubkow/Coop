using AutoMapper;

namespace Coop.Application.RealtyOwner
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Domain.Realties.RealtyOwner, RealtyOwnerViewModel>()
                .ForMember(r => r.OwnerId, m => m.MapFrom(
                    r => r.UserId))
                .ForMember(r => r.TransferDate, m => m.MapFrom(
                    r => r.TransferDate));
        }
    }
}