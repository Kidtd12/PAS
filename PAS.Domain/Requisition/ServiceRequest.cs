using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Requisition
{
    public class ServiceRequest : BaseEntity
    {
        public string SRNumber { get; private set; }

        public Guid RequesterId { get; private set; }

        public Guid? ApprovedById { get; private set; }

        public DateTime RequestDate { get; private set; }

        public string Status { get; private set; }

        public ICollection<SR_Detail> Details { get; private set; } = new List<SR_Detail>();
        public ServiceRequest(string number, Guid requester)
        {
            SRNumber = number;
            RequesterId = requester;
            RequestDate = DateTime.UtcNow;
            Status = "Pending";
        }

        public void Approve(Guid approver)
        {
            ApprovedById = approver;
            Status = "Approved";
        }
    }
}