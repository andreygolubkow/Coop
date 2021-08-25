using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coop.Web.Models;
using JetBrains.Annotations;

namespace Coop.Web.Data
{
    public interface IUserService
    {
        List<UserShortViewModel> GetUsers();
        Task<Guid> CreateUserAsync(CreateUserInputModel model, CancellationToken token);

        Task AddToRole(Guid user, string role, CancellationToken token);

        [CanBeNull] UserFullInfoViewModel GetUser(Guid id);

        Task ResetUserPasswordAsync(Guid id, string password, CancellationToken token);
    }
}