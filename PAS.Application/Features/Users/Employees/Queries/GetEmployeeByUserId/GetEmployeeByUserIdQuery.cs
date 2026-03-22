using Application.Common.Security;
using Application.Features.Users.Employees.Dtos;
using MediatR;
using PAS.Application.Features.Users.Employees.DTOs;

namespace Application.Features.Users.Employees.Queries;

[Authorize(Permissions = Permissions.Employees.View)]
public record GetEmployeeByUserIdQuery(Guid UserId) : IRequest<Result<EmployeeDetailDto>>;