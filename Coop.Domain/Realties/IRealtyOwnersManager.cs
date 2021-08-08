using System;

namespace Coop.Domain.Realties
{
    public interface IRealtyOwnersManager
    {
        public void SetOwnerTo(Realty realty, Guid userId);
    }
}