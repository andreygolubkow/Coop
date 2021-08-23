using System;
using System.Collections.Generic;

namespace Coop.Application.Realty
{
    public class RealtyFullViewModel
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public Guid OwnerId { get; set; }

        /// <summary>
        ///     Текущая задолженность в string т.е. не сделан маппинг в null
        /// </summary>
        public string CurrentDebt { get; set; }
        
        public List<RealtyPayViewModel> Pays { get; set; }
    }
}