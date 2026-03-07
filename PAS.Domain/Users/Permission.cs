
using Domain.Common;

namespace Domain.Users
{
    public class Permission : BaseEntity
    {
        public string PermissionName { get; private set; }

        public string Description { get; private set; }

        public Permission(string name, string description)
        {
            PermissionName = name;
            Description = description;
        }
    }
}
