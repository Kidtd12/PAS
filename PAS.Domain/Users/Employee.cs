using Domain.Common;

namespace Domain.Users
{
    public class Employee : BaseEntity
    {
        public string EmployeeCode { get; private set; }

        public string FullName { get; private set; }

        public string Department { get; private set; }

        private Employee()
        {
        }

        public Employee(string code, string name, string department)
        {
            EmployeeCode = code;
            FullName = name;
            Department = department;
        }
    }
}
