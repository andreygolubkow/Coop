using System;
using System.Collections.Generic;

namespace Coop.Application.RealtyOwner
{
    public interface IRealtyOwnerService
    {
        IEnumerable<RealtyOwnerViewModel> GetOwnersHistory(Guid realtyId);
    }
}