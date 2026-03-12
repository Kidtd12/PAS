using Domain.Common;

namespace Domain.PropertyManagement
{
    public class PropertyCategory : BaseEntity
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        private PropertyCategory()
        {
        }

        public PropertyCategory(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}