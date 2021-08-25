using System;
using System.ComponentModel.DataAnnotations;

namespace Coop.Web.Models
{
    public class ResetUserPasswordViewModel
    {
        public Guid UserId { get; set; }
        
        public string Error { get; set; }
    }
}