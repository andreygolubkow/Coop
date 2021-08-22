using Microsoft.Extensions.DependencyInjection;

namespace Coop.Application.RealtyOwner
{
    public static class Config
    {
        public static IServiceCollection AddRealtyOwnerFeature(this IServiceCollection collection)
        {
            collection.AddScoped<IRealtyOwnerService, RealtyOwnerService>();
            collection.AddAutoMapper(typeof(Mappings));
            return collection;
        }
    }
}