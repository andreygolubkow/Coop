using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ardalis.GuardClauses;
using Coop.Domain.Common;

namespace Coop.Domain.Realties
{
    /// <summary>
    /// Имущество.
    /// </summary>
    public class Realty: Entity
    {
        protected Realty(){}
        
        /// <summary>
        /// Инвентарный номер(например, номер гаража)
        /// Используем string т.к. могут быть литеры: 101а
        /// </summary> 
        public string InventoryNumber { get; protected set; }
        
        public bool IsActive { get; protected set; }

        public List<RealtyOwner> Owners { get; protected set; } = new List<RealtyOwner>();

        public List<RealtyDebt> Debts { get; protected set; } = new List<RealtyDebt>();

        public List<RealtyPay> Pays { get; protected set; } = new List<RealtyPay>();

        public static Realty Create(string inventoryNumber)
        {
            Guard.Against.NullOrWhiteSpace(inventoryNumber, nameof(inventoryNumber), "Инвентарный номер не может быть пустым");

            return new Realty()
            {
                InventoryNumber = inventoryNumber,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Id = Guid.NewGuid()
            };
        }

        public void Archive()
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("Данный объект уже в архиве");
            }

            IsActive = false;
            UpdatedAt = DateTime.Now;
        }

        public void ChangeInventoryNumber(string newInventoryNumber)
        {
            Guard.Against.NullOrWhiteSpace(newInventoryNumber, nameof(newInventoryNumber),
                "Необходимо указать инвентарный номер");

            if (!IsActive)
            {
                throw new InvalidOperationException("Объект в архиве и его нельзя изменить");
            }
            
            InventoryNumber = newInventoryNumber;
            UpdatedAt = DateTime.Now;
        }

        public RealtyOwner SetOwner(Guid newOwnerId, DateTime transferDate)
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("Объект в архиве и ему нельзя назначить владельца");
            }

            Owners ??= new List<RealtyOwner>();
            var owner = new RealtyOwner(transferDate, newOwnerId, this.Id);
            Owners.Add(owner);
            return owner;
        }

        public void SetBalance(IBillingManager billingManager, decimal balance, DateTimeOffset balanceTimestamp)
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("Объект в архиве и для него нельзя установить баланс");
            }
            
            billingManager.SetCurrentBalanceFor(this, balance, balanceTimestamp);
        }

        public void AddPay(IPayManager payManager, DateTimeOffset dateTime, decimal money)
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("Объект в архиве и для него нельзя установить баланс");
            }

            Guard.Against.NegativeOrZero(money, nameof(money), "В сумме платежа разрешены только положительные числа");
            
            payManager.AddPay(this, dateTime, money);
        }

        public Guid? GetCurrentOwnerId()
        {
            if (Owners == null || Owners.Count == 0) return null;
            var maxDt = Owners.Max(o => o.TransferDate);
            return Owners.First(o => o.TransferDate == maxDt).UserId;
        }
    }
}