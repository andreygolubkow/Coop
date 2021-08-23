using System;

namespace Coop.Web.PaysParser
{
    public class PayRecord
    {
        public string InventoryNumber { get; init; }
        
        public string PayeerName { get; init; }
        
        public DateTime PayTimestamp { get; init; }
        
        public decimal Amount { get; init; }
    }
}