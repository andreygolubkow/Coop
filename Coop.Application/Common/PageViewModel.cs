using System.Collections.Generic;

namespace Coop.Application.Common
{
    public class PageViewModel<T>
    {
        /// <summary>
        /// Текущая страница.
        /// </summary>
        public int CurrentPage { get; set; }
        
        /// <summary>
        /// Всего страниц.
        /// </summary>
        public int TotalPages { get; set; }
        
        /// <summary>
        /// Размер страницы.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Элементы страницы.
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();
    }
}