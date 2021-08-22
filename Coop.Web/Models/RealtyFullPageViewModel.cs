using System;
using System.Collections.Generic;
using Coop.Application.Realty;
using RealtyOwnerViewModel = Coop.Application.RealtyOwner.RealtyOwnerViewModel;

namespace Coop.Web.Models
{
    public class RealtyFullPageViewModel
    {
        public RealtyFullViewModel Realty { get; set; }

        public IEnumerable<RealtyOwnerViewModel> OwnHistory { get; set; } = new List<RealtyOwnerViewModel>();

        public IDictionary<Guid, string> Users { get; set; } = new Dictionary<Guid, string>();
    }


    public class RealtyPayRecord
    {
        public DateTime Timestamp { get; set; }

        public string Payer { get; set; }

        public decimal Amount { get; set; }
    }
}