using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coop.Web.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Coop.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            if (args.Contains("/seed")) await Seed(host);

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }

        public static async Task Seed(IHost host)
        {
            var userManager = host.Services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = host.Services.GetRequiredService<RoleManager<ApplicationRole>>();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            var config = host.Services.GetRequiredService<IConfiguration>();
            var users = config.GetSection("Users").Get<List<User>>();

            if (users == null)
            {
                logger.LogError("Запрошено создание пользователей, но список пользователей не найден в секции Users");
                return;
            }

            foreach (var user in users)
            {
                if (string.IsNullOrWhiteSpace(user.Email)
                    || string.IsNullOrWhiteSpace(user.Password)
                    || string.IsNullOrWhiteSpace(user.Role))
                {
                    logger.LogWarning(
                        $"Пользователь {user.Email} не добавлен, отсутствует одно из обязательных свойств Name,Password, Role");
                    continue;
                }

                if (user.Role != Constants.ADMIN_ROLE && user.Role != Constants.USER_ROLE)
                {
                    logger.LogWarning(
                        $"Для пользователя {user.Email} указана неверная роль {user.Role}. Доступные: {Constants.USER_ROLE} {Constants.ADMIN_ROLE}");
                    continue;
                }

                var identityUser = new ApplicationUser(user.Email, "Пользователь", "-","-")
                {
                    UserName = user.Email,
                    Email = user.Email,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(identityUser, user.Password);
                if (!result.Succeeded)
                {
                    logger.LogError(
                        $"Пользователь {user.Email} не добавлен: {string.Join(' ', result.Errors.Select(e => e.Description))}");
                    continue;
                }

                var role = await roleManager.FindByNameAsync(user.Role);
                if (role == null) await roleManager.CreateAsync(new ApplicationRole(user.Role));
                result = await userManager.AddToRoleAsync(identityUser, user.Role);
                if (!result.Succeeded) logger.LogError($"Пользователь {user.Email} не добавлен в группу {user.Role}");
            }

            logger.LogInformation("Выполнено добавление пользователей из конфигурации");
        }
    }

    public class User
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}