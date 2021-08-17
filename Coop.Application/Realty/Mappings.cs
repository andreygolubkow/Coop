using AutoMapper;

namespace Coop.Application.Realty
{
    public class Mappings: Profile
    {
        public Mappings()
        {
            CreateMap<Domain.Realties.Realty, RealtyListItemViewModel>()
                .ForMember(r => r.Id, m => m.MapFrom(
                    r => r.Id))
                .ForMember(r => r.InventoryNumber, m => m.MapFrom(
                    r => r.InventoryNumber));
        }
    }
}