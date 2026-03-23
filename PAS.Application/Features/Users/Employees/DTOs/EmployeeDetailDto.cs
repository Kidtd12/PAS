using Application.Common.Mappings;
using AutoMapper;
using System;

namespace Application.Features.Users.Employees.Dtos
{
    public class EmployeeDetailDto : IMapFrom<Domain.Users.Employee>
    {
        public Guid Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string? Email { get; set; }
        public bool IsActive { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Users.Employee, EmployeeDetailDto>();
        }
    }

    public class EmployeeListDto
    {
        public Guid Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
