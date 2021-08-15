using Coop.Application.Articles;
using Microsoft.Extensions.DependencyInjection;

namespace Coop.Application.Advertisement
{
    public static class Config
    {
        public static IServiceCollection AddAdvertisementFeature(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Mappings));
           return services.AddTransient<IAdvertisementService, AdvertisementService>();
        }
    }
}