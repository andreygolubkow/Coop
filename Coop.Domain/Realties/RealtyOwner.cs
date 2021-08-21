using System;

namespace Coop.Domain.Realties
{
    public class RealtyOwner
    {
        protected RealtyOwner(){}

        public RealtyOwner(DateTime transferDate, Guid userId, Guid realtyId)
        {
            UserId = userId;
            TransferDate = transferDate;
            RealtyId = realtyId;
        }

        public Guid Id { get; protected set; } = Guid.NewGuid();

        public Guid RealtyId { get; protected set; }
        
        public Guid UserId { get; protected set; }

        public DateTime TransferDate
        {
            get;
            set;
        }
}
}