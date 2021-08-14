using System;
using System.ComponentModel.DataAnnotations;

namespace Coop.Application.Articles
{
    public class UpdateArticleInputModel: CreateArticleInputModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}