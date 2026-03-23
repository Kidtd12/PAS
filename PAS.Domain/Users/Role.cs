using Domain.Common;

namespace Domain.Users
{
    public class Role : BaseEntity
    {
        public string RoleName { get; private set; }

        public string Description { get; private set; }

        public ICollection<UserLogin> Users { get; private set; } = new List<UserLogin>();

        private Role()
        {
        }

        public Role(string name, string description)
        {
            RoleName = name;
            Description = description;
        }
    }
}