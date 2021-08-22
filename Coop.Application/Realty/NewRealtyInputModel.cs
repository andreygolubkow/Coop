using System.ComponentModel.DataAnnotations;

namespace Coop.Application.Realty
{
    public class NewRealtyInputModel
    {
        [Required(ErrorMessage = "Необходимо указать номер")]
        [MinLength(1, ErrorMessage = "Необходимо указать номер")]
        public string InventoryNumber { get; set; }
    }
}