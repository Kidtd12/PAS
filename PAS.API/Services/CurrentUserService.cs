using Application.Common.Interfaces;
using Application.Constants;
using System.Security.Claims;

namespace PAS.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _context;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public Guid? UserGuid
    {
        get
        {
            if (Guid.TryParse(UserId, out var guid))
                return guid;
            return null;
        }
    }

    public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public IEnumerable<string> Roles => _httpContextAccessor.HttpContext?.User?
        .FindAll(ClaimTypes.Role).Select(c => c.Value) ?? Enumerable.Empty<string>();

    public IEnumerable<string> Permissions
    {
        get
        {
            var user = _context.UserLogins
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Id == UserGuid && !u.IsDeleted);

            if (user?.Role == null)
                return Enumerable.Empty<string>();

            // Get permissions based on role
            return user.Role.RoleName switch
            {
                SystemRoles.Admin => SystemRoles.RolePermissions.AdminPermissions,
                SystemRoles.StoreOfficer => SystemRoles.RolePermissions.StoreOfficerPermissions,
                SystemRoles.Staff => SystemRoles.RolePermissions.StaffPermissions,
                SystemRoles.Inspector => SystemRoles.RolePermissions.InspectorPermissions,
                SystemRoles.Approver => SystemRoles.RolePermissions.ApproverPermissions,
                _ => Enumerable.Empty<string>()
            };
        }
    }

    public bool IsAuthenticated => UserId != null;

    public bool IsInRole(string role)
    {
        return Roles.Contains(role);
    }

    public bool HasPermission(string permission)
    {
        return Permissions.Contains(permission);
    }
}