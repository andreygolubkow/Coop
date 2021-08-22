using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coop.Application.AdminNotes
{
    public static class Config
    {
        public const string DefaultSection = "Contacts";

        public static IServiceCollection AddCompanyInformationFeature(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<CompanyInformationOptions>(a => { configuration.GetSection(DefaultSection).Bind(a); });
            services.AddTransient<ICompanyInformation, CompanyInformation>();
            return services;
        }
    }
}