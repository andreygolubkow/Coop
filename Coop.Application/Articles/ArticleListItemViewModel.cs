using System;

namespace Coop.Application.Articles
{
    /// <summary>
    /// Одна новости из списка новостей.
    /// </summary>
    public class ArticleListItemViewModel
    {
        /// <summary>
        /// Идентификатор новости.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// Дата-время создания новости.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Текст новости.
        /// </summary>
        public string Details { get; set; } = string.Empty;
    }
}