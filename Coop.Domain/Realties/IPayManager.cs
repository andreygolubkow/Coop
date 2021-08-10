using System;

namespace Coop.Domain.Realties
{
    public interface IPayManager
    {
        public void AddPay(Realty realty, DateTimeOffset dateTime, decimal money);
    }
}