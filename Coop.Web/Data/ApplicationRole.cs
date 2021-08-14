using System;
using Microsoft.AspNetCore.Identity;

namespace Coop.Web.Data
{
    public class ApplicationRole: IdentityRole<Guid>
    {
        public ApplicationRole(string roleName): base(roleName)
        {
            
        }

        public ApplicationRole()
        {
            
        }
    }
}