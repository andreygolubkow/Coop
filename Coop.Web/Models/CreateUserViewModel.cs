using System;
using System.ComponentModel.DataAnnotations;

namespace Coop.Web.Models
{
    public class CreateUserViewModel
    {
        public Guid CreatedGuid { get; set; }
        
        public string Error { get; set; }

        public CreateUserInputModel InputModel { get; set; } = new CreateUserInputModel();
    }

    public class CreateUserInputModel
    {
        [EmailAddress(ErrorMessage = "Введите электронную почту в формате user@email.com")]
        [Required(ErrorMessage = "Введите электронную почту")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Введитие ФИО")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Введитие адрес")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "Введитие телефон")]
        public string Phone { get; set; }
        
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }
        
        public bool IsAdmin { get; set; }
    }
}