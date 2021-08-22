using System;

namespace Coop.Application.Realty
{
    public class RealtyListItemViewModel
    {
        public Guid Id { get; set; }

        public string InventoryNumber { get; set; }

        /// <summary>
        /// Задолженность в формате строки
        /// </summary>
        public string Balance { get; set; }
        
        public Guid? OwnerId { get; set; }
    }
}