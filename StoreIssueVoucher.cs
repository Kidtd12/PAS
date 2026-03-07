using System;
using Domain.Common;

namespace Domain.Requisition
{
    public class StoreIssueVoucher : BaseEntity
    {
        public Guid SRId { get; private set; }

        public string SIVNumber { get; private set; }

        public DateTime IssueDate { get; private set; }

        public Guid IssuedById { get; private set; }

        public string RecipientSignature { get; private set; }

        public StoreIssueVoucher(Guid srId, Guid issuedBy, string signature)
        {
            SRId = srId;
            IssuedById = issuedBy;
            RecipientSignature = signature;
            IssueDate = DateTime.UtcNow;
        }
    }
}
