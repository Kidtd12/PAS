using System;
using Domain.Catalog;
using Domain.Common;
using Domain.PropertyManagement;

namespace Domain.TransferReturn
{
    public class TransferRecord : BaseEntity
    {
        public Guid ItemId { get; private set; }

        public Guid FromLocationId { get; private set; }

        public Guid ToLocationId { get; private set; }

        public int Quantity { get; private set; }

        public DateTime TransferDate { get; private set; }

        public ItemMaster Item { get; private set; }

        public PropertyLocation FromLocation { get; private set; }

        public PropertyLocation ToLocation { get; private set; }

        private TransferRecord()
        {
        }

        public TransferRecord(Guid itemId, Guid from, Guid to, int qty)
        {
            ItemId = itemId;
            FromLocationId = from;
            ToLocationId = to;
            Quantity = qty;
            TransferDate = DateTime.UtcNow;
        }
    }
}
