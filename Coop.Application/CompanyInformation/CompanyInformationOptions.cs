using System.Collections.Generic;

namespace Coop.Application.AdminNotes
{
    public class CompanyInformationOptions
    {
        /// <summary>
        ///     Заголовок блока (Название компании)
        /// </summary>
        public string Company { get; set; } = string.Empty;

        /// <summary>
        ///     Реквизиты в виде списка
        /// </summary>
        public List<string> Properties { get; set; } = new();

        /// <summary>
        ///     Контакты администрации.
        /// </summary>
        public List<CompanyContactOptions> Contacts { get; set; } = new();
    }

    public class CompanyContactOptions
    {
        /// <summary>
        ///     Название канала связи
        /// </summary>
        public string Channel { get; set; } = string.Empty;

        /// <summary>
        ///     Ссылка, номер телефона и т д
        /// </summary>
        public string Address { get; set; } = string.Empty;
    }
}