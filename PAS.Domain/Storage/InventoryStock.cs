using System;
using Domain.Common;
using Domain.Catalog;

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

        // Navigation properties
        public ItemMaster? Item { get; private set; }

        public ShelfLocation? ShelfLocation { get; private set; }

        public bool CheckAvailability(int qty)
        {
            return (CurrentQuantity - ReservedQuantity) >= qty;
        }
    }
}