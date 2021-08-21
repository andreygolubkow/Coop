﻿using System;

namespace Coop.Application.Realty
{
    public class RealtyListItemViewModel
    {
        public Guid Id { get; set; }
        
        public string InventoryNumber { get; set; }
        
        public Guid? OwnerId { get; set; }
        
    }
}