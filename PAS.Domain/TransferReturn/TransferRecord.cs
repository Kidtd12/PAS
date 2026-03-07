using System;
using Domain.Common;

namespace Domain.TransferReturn
{
    public class TransferRecord : BaseEntity
    {
        public Guid ItemId { get; private set; }

        public Guid FromLocationId { get; private set; }

        public Guid ToLocationId { get; private set; }

        public int Quantity { get; private set; }

        public DateTime TransferDate { get; private set; }

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
