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

        // Navigation properties
        public PropertyType PropertyType { get; private set; }

        public PropertyLocation Location { get; private set; }

        public SafetyBox SafetyBox { get; private set; }

        public Property(string tagNumber, string name, string serialNumber, Guid propertyTypeId,
            decimal unitPrice, int quantity, DateTime purchaseDate, Guid locationId, Guid? safetyBoxId)
        {
            TagNumber = tagNumber;
            Name = name;
            SerialNumber = serialNumber;
            PropertyTypeId = propertyTypeId;
            UnitPrice = unitPrice;
            Quantity = quantity;
            PurchaseDate = purchaseDate;
            LocationId = locationId;
            SafetyBoxId = safetyBoxId;
        }
    }
}