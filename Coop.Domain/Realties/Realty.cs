﻿using System;
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

        public static Realty Create(string inventoryNumber)
        {
            Guard.Against.NullOrWhiteSpace(inventoryNumber, nameof(inventoryNumber), "Инвентарный номер не может быть пустым");

            return new Realty()
            {
                InventoryNumber = inventoryNumber,
                IsActive = true,
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now,
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
            UpdatedAt = DateTimeOffset.Now;
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
            UpdatedAt = DateTimeOffset.Now;
        }

        public void SetOwner(IRealtyOwnersManager manager, Guid newOwnerId)
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("Объект в архиве и ему нельзя назначить владельца");
            }
            
            manager.SetOwnerTo(this, newOwnerId);
        }

        public void SetBalance(IBillingManager billingManager, decimal balance, DateTimeOffset balanceTimestamp)
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("Объект в архиве и для него нельзя установить баланс");
            }
            
            billingManager.SetCurrentBalanceFor(this, balance, balanceTimestamp);
        }
    }
}