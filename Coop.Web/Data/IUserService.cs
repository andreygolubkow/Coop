using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coop.Web.Models;

namespace Coop.Web.Data
{
    public interface IUserService
    {
        List<UserShortViewModel> GetUsers();
        Task<Guid> CreateUserAsync(CreateUserInputModel model, CancellationToken token);

        Task AddToRole(Guid user, string role, CancellationToken token);
    }
}