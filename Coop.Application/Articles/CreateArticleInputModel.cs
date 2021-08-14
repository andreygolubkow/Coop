using System;
using System.ComponentModel.DataAnnotations;

namespace Coop.Application.Articles
{
    public class CreateArticleInputModel
    {
        [MinLength(1, ErrorMessage = "Введите заголовок")]
        [MaxLength(100, ErrorMessage = "Заголовок слишком длинный, введите до 100 знаков")]
        [Required(ErrorMessage = "Введите заголовок")]
        public string Title { get; set; }
        [MinLength(1, ErrorMessage = "Введите текст новости")]
        [Required(ErrorMessage = "Введите текст новости")]
        public string Text { get; set; }
    }
}