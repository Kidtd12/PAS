namespace Application.Features.Users.Employees.Queries;

public class GetEmployeeByUserIdQueryHandler : IRequestHandler<GetEmployeeByUserIdQuery, Result<EmployeeDetailDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEmployeeByUserIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<EmployeeDetailDto>> Handle(GetEmployeeByUserIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.UserLogins
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted, cancellationToken);

        if (user?.Employee == null)
        {
            throw new NotFoundException(nameof(Domain.Users.Employee), $"User ID {request.UserId}");
        }

        var employeeDto = _mapper.Map<EmployeeDetailDto>(user.Employee);

        // Map user account
        employeeDto.UserAccount = new UserAccountSummaryDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email ?? string.Empty,
            Role = user.Role?.RoleName ?? "Unknown",
            IsActive = user.IsActive,
            LastLoginAt = user.LastLoginAt
        };

        return Result<EmployeeDetailDto>.Success(employeeDto);
    }
}