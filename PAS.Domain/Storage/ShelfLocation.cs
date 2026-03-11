using System;
using Domain.Common;

namespace Domain.Storage
{
    public class ShelfLocation : BaseEntity
    {
        public Guid WarehouseId { get; private set; }

        public string Aisle { get; private set; }

        public string Rack { get; private set; }

        public string ShelfNumber { get; private set; }

        public string QRCodeValue { get; private set; }

        public Warehouse? Warehouse { get; private set; }

        private ShelfLocation()
        {
        }

        public ShelfLocation(Guid warehouseId, string aisle, string rack, string shelf)
        {
            WarehouseId = warehouseId;
            Aisle = aisle;
            Rack = rack;
            ShelfNumber = shelf;
        }
    }
}