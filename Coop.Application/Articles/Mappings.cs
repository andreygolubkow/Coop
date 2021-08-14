using AutoMapper;
using Coop.Domain.Articles;

namespace Coop.Application.Articles
{
    public class Mappings: Profile
    {
        public Mappings()
        {
            CreateMap<Article, ArticleListItemViewModel>()
                .ForMember(a => a.Id, m => m.MapFrom(
                    a => a.Id))
                .ForMember(a => a.Title, m => m.MapFrom(
                    a => a.Title))
                .ForMember(a => a.Details, m => m.MapFrom(
                    a => a.Text))
                .ForMember(a => a.CreatedAt, m => m.MapFrom(
                    a => a.CreatedAt));

            CreateMap<Article, UpdateArticleInputModel>()
                .ForMember(a => a.Id, m => m.MapFrom(
                    a => a.Id))
                .ForMember(a => a.Title, m => m.MapFrom(
                    a => a.Title))
                .ForMember(a => a.Text, m => m.MapFrom(
                    a => a.Text));
        }
    }
}