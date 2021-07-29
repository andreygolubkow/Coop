using System;

namespace Coop.Application.News
{
    /// <summary>
    /// Одна новости из списка новостей.
    /// </summary>
    public class NewsListItemViewModel
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
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Текст новости.
        /// </summary>
        public string Details { get; set; } = string.Empty;
    }
}