using System;
using Domain.Common;

namespace Domain.Users
{
    public class UserLogin : BaseEntity
    {
        public Guid EmployeeId { get; private set; }

        public string Username { get; private set; }

        public string PasswordHash { get; private set; }

        public Guid RoleId { get; private set; }

        public string? AspNetUserId { get; private set; }

        public bool IsActive { get; private set; }

        public Employee Employee { get; private set; }

        public Role Role { get; private set; }

        private UserLogin()
        {
        }

        public UserLogin(Guid employeeId, string username, string password, Guid roleId, string? aspNetUserId = null)
        {
            EmployeeId = employeeId;
            Username = username;
            PasswordHash = password;
            RoleId = roleId;
            AspNetUserId = aspNetUserId;
            IsActive = true;
        }
    }
}