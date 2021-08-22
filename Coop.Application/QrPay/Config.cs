using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coop.Application.QrPay
{
    public static class Config
    {
        public const string DefaultConfigSection = "QrPay";

        public static IServiceCollection AddQrPayFeature(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<QrPayOptions>(a => { configuration.GetSection(DefaultConfigSection).Bind(a); });
            services.AddTransient<IQrPay, QrPay>();
            return services;
        }
    }
}