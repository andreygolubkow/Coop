using Microsoft.Extensions.DependencyInjection;

namespace Coop.Application.Articles
{
    public static class Config
    {
        public static IServiceCollection AddArticleFeature(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Mappings));
           return services.AddTransient<IArticleService, ArticleService>();
        }
    }
}