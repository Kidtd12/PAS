
using Domain.Common;

namespace Domain.PropertyManagement
{
    public class PropertyType : BaseEntity
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public PropertyType(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}