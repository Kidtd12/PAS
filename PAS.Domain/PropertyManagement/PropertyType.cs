using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> origin/kid-application
using Domain.Common;

namespace Domain.PropertyManagement
{
    public class PropertyType : BaseEntity
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public Guid? PropertyCategoryId { get; private set; }

        public PropertyCategory PropertyCategory { get; private set; }

<<<<<<< HEAD
=======
        public ICollection<Property> Properties { get; private set; } = new List<Property>();

>>>>>>> origin/kid-application
        private PropertyType()
        {
        }

        public PropertyType(string name, string description, Guid? propertyCategoryId = null)
        {
            Name = name;
            Description = description;
            PropertyCategoryId = propertyCategoryId;
        }
    }
}