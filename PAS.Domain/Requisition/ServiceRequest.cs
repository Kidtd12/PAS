using System;
using System.Collections.Generic;
using Domain.Common;
<<<<<<< HEAD
=======
using Domain.Users;
>>>>>>> origin/kid-application
using Domain.Workflow;

namespace Domain.Requisition
{
    public class ServiceRequest : BaseEntity
    {
        public string SRNumber { get; private set; }

        public Guid RequesterId { get; private set; }

        public Guid? ApprovedById { get; private set; }

        public Guid? ApprovalStatusId { get; private set; }

        public DateTime RequestDate { get; private set; }

        public string Status { get; private set; }

<<<<<<< HEAD
=======
        public UserLogin Requester { get; private set; }

        public UserLogin ApprovedBy { get; private set; }

>>>>>>> origin/kid-application
        public ApprovalStatus ApprovalStatus { get; private set; }

        public ICollection<SR_Detail> Details { get; private set; } = new List<SR_Detail>();

        private ServiceRequest()
        {
        }

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