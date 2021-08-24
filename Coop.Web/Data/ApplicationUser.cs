using System;
using Microsoft.AspNetCore.Identity;

namespace Coop.Web.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser(string userName, string fullName, string address, string phone) : base(userName)
        {
            FullName = fullName;
            Address = address;
            Phone = phone;
        }

        public ApplicationUser()
        {
        }

        public string FullName { get; protected set; }
        
        public string Phone { get; protected set; }
        
        public string Address { get; protected set; }
        
    }
}