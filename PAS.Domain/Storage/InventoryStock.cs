using System;
using Domain.Common;

namespace Domain.Storage
{
    public class InventoryStock : BaseEntity
    {
        public Guid ItemId { get; private set; }

        public Guid ShelfId { get; private set; }

        public int CurrentQuantity { get; private set; }

        public int ReservedQuantity { get; private set; }

        public InventoryStock(Guid itemId, Guid shelfId, int quantity)
        {
            ItemId = itemId;
            ShelfId = shelfId;
            CurrentQuantity = quantity;
        }

        public bool CheckAvailability(int qty)
        {
            return (CurrentQuantity - ReservedQuantity) >= qty;
        }
    }
}