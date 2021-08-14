using System;
using Microsoft.AspNetCore.Identity;

namespace Coop.Web.Data
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public ApplicationUser(string userName): base(userName)
        {
            
        }

        public ApplicationUser()
        {
            
        }
    }
}