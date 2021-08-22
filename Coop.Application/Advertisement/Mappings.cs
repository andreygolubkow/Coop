using AutoMapper;

namespace Coop.Application.Advertisement
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Domain.Advertisements.Advertisement, AdvertisementListItemViewModel>()
                .ForMember(a => a.Id, m => m.MapFrom(
                    a => a.Id))
                .ForMember(a => a.Title, m => m.MapFrom(
                    a => a.Title))
                .ForMember(a => a.Text, m => m.MapFrom(
                    a => a.Text))
                .ForMember(a => a.CreatedAt, m => m.MapFrom(
                    a => a.CreatedAt))
                .ForMember(a => a.AuthorId, m => m.MapFrom(
                    a => a.AuthorId))
                .ForMember(a => a.IsPublished, m => m.MapFrom(
                    a => a.IsPublished));
        }
    }
}