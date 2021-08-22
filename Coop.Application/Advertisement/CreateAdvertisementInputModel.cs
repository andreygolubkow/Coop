using System;
using System.ComponentModel.DataAnnotations;

namespace Coop.Application.Advertisement
{
    public class CreateAdvertisementInputModel
    {
        [MinLength(1, ErrorMessage = "Введите заголовок")]
        [MaxLength(100, ErrorMessage = "Заголовок слишком длинный, введите до 100 знаков")]
        [Required(ErrorMessage = "Введите заголовок")]
        public string Title { get; set; }

        [MinLength(1, ErrorMessage = "Введите текст объявления")]
        [MaxLength(1000, ErrorMessage = "Текст слишком длинный. Введите до 1000 знаков")]
        [Required(ErrorMessage = "Введите текст")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Пользователь не идентифицирован")]
        public Guid AuthorId { get; set; }
    }
}