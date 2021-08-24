using System;

namespace Coop.Web.Models
{
    public class UserShortViewModel
    {
        public Guid Id { get; set; }
        
        public string FullName { get; set; }
        
        public string Address { get; set; }
        
        public  string Phone { get; set; }
    }
}