using System;
using Domain.Common;

namespace Domain.PropertyManagement
{
    public class Property : BaseEntity
    {
        public string TagNumber { get; private set; }

        public string Name { get; private set; }

        public string SerialNumber { get; private set; }

        public Guid PropertyTypeId { get; private set; }

        public decimal UnitPrice { get; private set; }

        public int Quantity { get; private set; }

        public DateTime PurchaseDate { get; private set; }

        public Guid LocationId { get; private set; }

        public Guid? SafetyBoxId { get; private set; }

        public Property(string tag, string name, string serial, Guid typeId,
            decimal price, int qty, DateTime purchase, Guid locationId, Guid? boxId)
        {
            TagNumber = tag;
            Name = name;
            SerialNumber = serial;
            PropertyTypeId = typeId;
            UnitPrice = price;
            Quantity = qty;
            PurchaseDate = purchase;
            LocationId = locationId;
            SafetyBoxId = boxId;
        }
    }
}