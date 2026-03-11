using Domain.Common;

namespace Domain.Receiving
{
    public class Supplier : BaseEntity
    {
        public string SupplierName { get; private set; }

        public string ContactPerson { get; private set; }

        public string TinNumber { get; private set; }

        private Supplier()
        {
        }

        public Supplier(string name, string contact, string tin)
        {
            SupplierName = name;
            ContactPerson = contact;
            TinNumber = tin;
        }
    }
}