using Coop.Application.News;
using Microsoft.Extensions.DependencyInjection;

namespace Coop.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNewsFeature(this IServiceCollection services)
        {
           return services.AddTransient<INewsService, NewsService>();
        }
    }
}