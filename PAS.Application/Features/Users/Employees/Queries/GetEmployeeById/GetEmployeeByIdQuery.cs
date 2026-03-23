namespace Application.Features.Users.Employees.Queries;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDetailDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEmployeeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<EmployeeDetailDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .Include(e => e.UserLogin)
                .ThenInclude(u => u.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id && !e.IsDeleted, cancellationToken);

        if (employee == null)
        {
            throw new NotFoundException(nameof(Domain.Users.Employee), request.Id);
        }

        var employeeDto = _mapper.Map<EmployeeDetailDto>(employee);

        // Map user account if exists
        if (employee.UserLogin != null && !employee.UserLogin.IsDeleted)
        {
            employeeDto.UserAccount = new UserAccountSummaryDto
            {
                Id = employee.UserLogin.Id,
                Username = employee.UserLogin.Username,
                Email = employee.UserLogin.Email ?? string.Empty,
                Role = employee.UserLogin.Role?.RoleName ?? "Unknown",
                IsActive = employee.UserLogin.IsActive,
                LastLoginAt = employee.UserLogin.LastLoginAt
            };
        }

        // Get recent activities
        if (employee.UserLogin != null)
        {
            var activities = await _context.AuditTrails
                .Where(a => a.UserId == employee.UserLogin.Id)
                .OrderByDescending(a => a.ActionDate)
                .Take(10)
                .Select(a => new EmployeeActivityDto
                {
                    Date = a.ActionDate,
                    Action = a.Action,
                    Entity = a.EntityName,
                    Description = $"{a.Action} {a.EntityName}"
                })
                .ToListAsync(cancellationToken);

            employeeDto.RecentActivities = activities;
        }

        return Result<EmployeeDetailDto>.Success(employeeDto);
    }
}