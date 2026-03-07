using Domain.Common;

namespace ARMS.Domain.Disposal
{
    public class DisposalRecord : BaseEntity
    {
        public Guid ItemId { get; private set; }

        public int Quantity { get; private set; }

        public DateTime DisposalDate { get; private set; }

        public Guid DisposedBy { get; private set; }

        public string Reason { get; private set; } = string.Empty;
        public DateTime UpdatedAt { get; private set; }

        public DisposalRecord(Guid itemId, int quantity, Guid disposedBy, string? reason = null)
        {
            ItemId = itemId;
            Quantity = quantity;
            DisposedBy = disposedBy;
            Reason = reason ?? string.Empty;
            DisposalDate = DateTime.UtcNow;
        }

        public void UpdateReason(string reason)
        {
            Reason = reason ?? string.Empty;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}