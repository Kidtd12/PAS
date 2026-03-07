using System;
using Domain.Common;

namespace Domain.Catalog
{
    public class ItemMaster : BaseEntity
    {
        public string SKU { get; private set; }

        public string ItemName { get; private set; }

        public Guid CategoryId { get; private set; }

        public string UnitOfMeasure { get; private set; }

        public bool RequiresInspection { get; private set; }

        public int MinStockLevel { get; private set; }

        public ItemMaster(string sku, string itemName, Guid categoryId, string uom, bool inspection, int minStock)
        {
            SKU = sku;
            ItemName = itemName;
            CategoryId = categoryId;
            UnitOfMeasure = uom;
            RequiresInspection = inspection;
            MinStockLevel = minStock;
        }
    }
}