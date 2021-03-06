using System.Text;
using Coop.Application.AdminNotes;
using Coop.Application.Advertisement;
using Coop.Application.Articles;
using Coop.Application.QrPay;
using Coop.Application.Realty;
using Coop.Application.RealtyOwner;
using Coop.Domain.Advertisements;
using Coop.Domain.Articles;
using Coop.Domain.Common;
using Coop.Domain.Realties;
using Coop.Web.Data;
using Coop.Web.DebtsParser;
using Coop.Web.PaysParser;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Coop.Web
{
    public class Startup
    {
        private const string CAPTCHA_KEY = "Develop key";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredUniqueChars = 1;
                })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            // Captcha
            services.AddDNTCaptcha(options =>
            {
                options.UseCookieStorageProvider()
                    .AbsoluteExpiration(10)
                    .ShowThousandsSeparators(false)
                    .WithNoise(25, 3)
                    .WithEncryptionKey(CAPTCHA_KEY)
                    .InputNames( // This is optional. Change it if you don't like the default names.
                        new DNTCaptchaComponent
                        {
                            CaptchaHiddenInputName = "SecureText",
                            CaptchaHiddenTokenName = "SecureToken",
                            CaptchaInputName = "SecureInput"
                        })
                    .Identifier("securityCheck")
                    ;
            });

            services.AddArticleFeature();
            services.AddAdvertisementFeature();
            services.AddCompanyInformationFeature(Configuration);
            services.AddQrPayFeature(Configuration);
            services.AddRealtyFeature();
            services.AddRealtyOwnerFeature();

            services.AddAutoMapper(GetType().Assembly);

            services.AddScoped<IRepository<Article>, RepositoryBase<Article>>();
            services.AddScoped<IRepository<Advertisement>, RepositoryBase<Advertisement>>();
            services.AddScoped<IRepository<Realty>, RepositoryBase<Realty>>();
            services.AddScoped<IRepository<RealtyOwner>, RepositoryBase<RealtyOwner>>();
            services.AddScoped<IRepository<RealtyDebt>, RepositoryBase<RealtyDebt>>();
            services.AddScoped<IRepository<RealtyPay>, RepositoryBase<RealtyPay>>();

            services.AddScoped<IRepository<ApplicationUser>, RepositoryBase<ApplicationUser>>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IDebtParser, DebtParser>();
            services.AddScoped<IPaysParser, PaysParser.PaysParser>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}