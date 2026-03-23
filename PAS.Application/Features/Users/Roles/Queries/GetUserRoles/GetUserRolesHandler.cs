using AutoMapper;
using MediatR;
using PAS.Application.Features.Users.Roles.Queries.GetUserRoles;

namespace Application.Features.Users.Roles.Queries;

public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, Result<UserRoleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserRolesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<UserRoleDto>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.UserLogins
            .Include(u => u.Employee)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(Domain.Users.UserLogin), request.UserId);
        }

        var allRoles = await _context.Roles
            .Where(r => !r.IsDeleted)
            .OrderBy(r => r.RoleName)
            .ToListAsync(cancellationToken);

        var result = new UserRoleDto
        {
            UserId = user.Id,
            Username = user.Username,
            EmployeeName = user.Employee?.FullName ?? "Unknown",
            Roles = allRoles.Select(r => new RoleAssignmentDto
            {
                RoleId = r.Id,
                RoleName = r.RoleName,
                IsAssigned = user.RoleId == r.Id
            }).ToList()
        };

        return Result<UserRoleDto>.Success(result);
    }
}