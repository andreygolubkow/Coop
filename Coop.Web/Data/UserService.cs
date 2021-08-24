using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coop.Domain.Common;
using Coop.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Coop.Web.Data
{
    public class UserService: IUserService
    {
        private readonly IRepository<ApplicationUser> _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IRepository<ApplicationUser> repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        
        public List<UserShortViewModel> GetUsers()
        {
            return _repository.GetAll().AsNoTracking()
                .OrderBy(r => r.FullName)
                .Select(r => new UserShortViewModel()
                {
                    Id = r.Id,
                    FullName = r.FullName,
                    Address = r.Address,
                    Phone = r.Phone
                }).ToList();
        }

        public async Task<Guid> CreateUserAsync(CreateUserInputModel model, CancellationToken token)
        {
            var result = await _userManager.CreateAsync(new ApplicationUser(model.Username, model.FullName, model.Address, model.Phone)
            {
                EmailConfirmed = true
            });
            if (!result.Succeeded)
            {
                throw new Exception(String.Join(" ", result.Errors.Select(e => e.Description)));
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            await _userManager.AddPasswordAsync(user, model.Password);
            return user.Id;
        }

        public async Task AddToRole(Guid user, string role, CancellationToken token)
        {
            var applicationUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == user, cancellationToken: token);
            await _userManager.AddToRoleAsync(applicationUser, role);
        }
    }
}