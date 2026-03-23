using AutoMapper;
using MediatR;
using Application.Features.Users.Roles.Dtos;

namespace Application.Features.Users.Roles.Queries;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result<List<RoleDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRolesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _context.Roles
            .Include(r => r.Users.Where(u => !u.IsDeleted))
            .Where(r => !r.IsDeleted)
            .OrderBy(r => r.RoleName)
            .ToListAsync(cancellationToken);

        var roleDtos = _mapper.Map<List<RoleDto>>(roles);

        if (request.IncludeUserCount)
        {
            foreach (var roleDto in roleDtos)
            {
                var role = roles.First(r => r.Id == roleDto.Id);
                roleDto.UserCount = role.Users?.Count ?? 0;
            }
        }

        // Add permissions based on role name
        foreach (var roleDto in roleDtos)
        {
            roleDto.Permissions = GetPermissionsForRole(roleDto.RoleName);
        }

        return Result<List<RoleDto>>.Success(roleDtos);
    }

    private List<string> GetPermissionsForRole(string roleName)
    {
        return roleName switch
        {
            SystemRoles.Admin => SystemRoles.RolePermissions.AdminPermissions.ToList(),
            SystemRoles.StoreOfficer => SystemRoles.RolePermissions.StoreOfficerPermissions.ToList(),
            SystemRoles.Staff => SystemRoles.RolePermissions.StaffPermissions.ToList(),
            SystemRoles.Inspector => SystemRoles.RolePermissions.InspectorPermissions.ToList(),
            SystemRoles.Approver => SystemRoles.RolePermissions.ApproverPermissions.ToList(),
            SystemRoles.Manager => SystemRoles.RolePermissions.ManagerPermissions.ToList(),
            _ => new List<string>()
        };
    }
}