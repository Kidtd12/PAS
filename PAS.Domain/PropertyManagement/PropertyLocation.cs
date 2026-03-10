using Domain.Common;

namespace Domain.PropertyManagement
{
    public class PropertyLocation : BaseEntity
    {
        public string Name { get; private set; }

        public string LocationType { get; private set; }

        public PropertyLocation(string name, string type)
        {
            Name = name;
            LocationType = type;
        }
    }
}