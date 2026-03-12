using System;
using Domain.Common;

namespace Domain.TransferReturn
{
    public class ReturnMaterialRequestNote : BaseEntity
    {
        public Guid ItemId { get; private set; }

        public int Quantity { get; private set; }

        public string Reason { get; private set; }

        public DateTime RequestDate { get; private set; }

        private ReturnMaterialRequestNote()
        {
        }

        public ReturnMaterialRequestNote(Guid itemId, int qty, string reason)
        {
            ItemId = itemId;
            Quantity = qty;
            Reason = reason;
            RequestDate = DateTime.UtcNow;
        }
    }
}

