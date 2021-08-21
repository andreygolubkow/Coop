using System;

namespace Coop.Domain.Realties
{
    /// <summary>
    /// Долги.
    /// </summary>
    public class RealtyDebt
    {
        protected RealtyDebt(){}

        public RealtyDebt(DateTime dateTime, decimal sum)
        {
            DateTime = dateTime;
            Sum = sum;
        }
        
        public Guid Id { get; protected set; }= Guid.NewGuid();
        
        public DateTime DateTime
        {
            get;
            protected set;
        }

        public decimal Sum { get; protected set; } = 0;
    }
}