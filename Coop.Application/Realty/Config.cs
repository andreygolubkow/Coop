using Microsoft.Extensions.DependencyInjection;

namespace Coop.Application.Realty
{
    public static class Config
    {
        public static IServiceCollection AddRealtyFeature(this IServiceCollection collection)
        {
            collection.AddScoped<IRealtyService,RealtyService>();
            collection.AddAutoMapper(typeof(Mappings));
            return collection;
        }
    }
}