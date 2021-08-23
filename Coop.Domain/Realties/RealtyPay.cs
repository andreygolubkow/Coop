using System;

namespace Coop.Domain.Realties
{
    public class RealtyPay
    {
        protected RealtyPay()
        {
        }

        public RealtyPay(DateTime dateTime, decimal paySum, string payerName)
        {
            DateTime = dateTime;
            PaySum = paySum;
            PayerName = payerName;
        }

        public Guid Id { get; protected set; } = Guid.NewGuid();

        public DateTime DateTime { get; protected set; }

        public string PayerName { get; protected set; }
        public decimal PaySum { get; protected set; }
    }
}