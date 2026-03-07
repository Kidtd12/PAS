using System;
using Domain.Common;

namespace Domain.Receiving
{
    public class ReceivingNote : BaseEntity
    {
        public string GRNNumber { get; private set; }

        public Guid SupplierId { get; private set; }

        public DateTime ReceivedDate { get; private set; }

        public string Status { get; private set; }

        public Guid ReceivedById { get; private set; }

        public ReceivingNote(string grn, Guid supplierId, Guid receiver)
        {
            GRNNumber = grn;
            SupplierId = supplierId;
            ReceivedById = receiver;
            ReceivedDate = DateTime.UtcNow;
            Status = "PendingInspection";
        }
    }
}