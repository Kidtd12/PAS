using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Catalog
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; }

        public Guid? ParentCategoryId { get; private set; }

        public string Description { get; private set; }

        public Category ParentCategory { get; private set; }

        public ICollection<Category> SubCategories { get; private set; } = new List<Category>();

        public Category(string name, string description, Guid? parentId = null)
        {
            Name = name;
            Description = description;
            ParentCategoryId = parentId;
        }
    }
}