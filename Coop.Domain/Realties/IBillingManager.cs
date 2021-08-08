using System;

namespace Coop.Domain.Realties
{
    public interface IBillingManager
    {
        /// <summary>
        /// Установить баланс счета для объекта собственности.
        /// </summary>
        /// <param name="realty"></param>
        /// <param name="currentBalance">Баланс счета</param>
        /// <param name="balanceTimestamp">Баланс актуален на момент времени</param>
        public void SetCurrentBalanceFor(Realty realty, decimal currentBalance, DateTimeOffset balanceTimestamp);
    }
}