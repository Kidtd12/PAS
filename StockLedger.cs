using System;
using Domain.Common;

namespace Domain.Storage
{
    public class StockLedger : BaseEntity
    {
        public Guid ItemId { get; private set; }

        public Guid ShelfId { get; private set; }

        public int QuantityChange { get; private set; }

        public string TransactionType { get; private set; }

        public Guid ReferenceId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public StockLedger(Guid itemId, Guid shelfId, int qty, string type, Guid refId)
        {
            ItemId = itemId;
            ShelfId = shelfId;
            QuantityChange = qty;
            TransactionType = type;
            ReferenceId = refId;
            CreatedDate = DateTime.UtcNow;
        }
    }
}