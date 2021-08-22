using System;
using Microsoft.AspNetCore.Identity;

namespace Coop.Web.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser(string userName, string fullName) : base(userName)
        {
            FullName = fullName;
        }

        public ApplicationUser()
        {
        }

        public string FullName { get; protected set; }
    }
}