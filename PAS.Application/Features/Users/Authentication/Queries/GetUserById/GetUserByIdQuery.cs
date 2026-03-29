using Application.Common.Security;
using Application.Features.Users.Authentication.Dtos;
using MediatR;

namespace Application.Features.Users.Authentication.Queries;

[Authorize(Permissions = Permissions.Users.View)]
public record GetUserByIdQuery(Guid Id) : IRequest<Result<UserDetailDto>>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUserByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<UserDetailDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.UserLogins
            .Include(u => u.Employee)
            .Include(u => u.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.Id && !u.IsDeleted, cancellationToken);

        if (user == null)
            return Result<UserDetailDto>.Failure("User not found.");

        var dto = new UserDetailDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email ?? string.Empty,
            IsActive = user.IsActive,
            RoleId = user.RoleId,
            RoleName = user.Role?.RoleName ?? string.Empty,
            EmployeeId = user.EmployeeId,
            EmployeeCode = user.Employee?.EmployeeCode ?? string.Empty,
            EmployeeName = user.Employee?.FullName ?? string.Empty
        };

        return Result<UserDetailDto>.Success(dto);
    }
}
