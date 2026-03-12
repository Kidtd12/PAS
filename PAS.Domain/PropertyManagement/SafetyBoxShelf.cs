using System;
using Domain.Common;

namespace Domain.PropertyManagement
{
    public class SafetyBoxShelf : BaseEntity
    {
        public Guid SafetyBoxId { get; private set; }

        public int ShelfNumber { get; private set; }

        private SafetyBoxShelf()
        {
        }

        public SafetyBoxShelf(Guid boxId, int shelf)
        {
            SafetyBoxId = boxId;
            ShelfNumber = shelf;
        }
    }
}