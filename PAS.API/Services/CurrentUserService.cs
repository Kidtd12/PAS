using Application.Common.Interfaces;
using Application.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PAS.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid? _resolvedUserId;
    private bool _resolved;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            if (_resolved)
            {
                return _resolvedUserId;
            }

            _resolved = true;

            var subject = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrWhiteSpace(subject))
            {
                _resolvedUserId = null;
                return _resolvedUserId;
            }

            _resolvedUserId = Guid.TryParse(subject, out var parsedId) ? parsedId : null;
            return _resolvedUserId;
        }
    }

    public string? Username => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
    public string? EmployeeCode => _httpContextAccessor.HttpContext?.User?.FindFirstValue("employeeCode");
    public string? EmployeeName => _httpContextAccessor.HttpContext?.User?.FindFirstValue("fullName");
    public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
    public Guid? RoleId => null;

    public IEnumerable<string> Permissions
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Role))
                return Enumerable.Empty<string>();

            return Role switch
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

    public bool IsAuthenticated => UserId.HasValue;
    public bool IsInRole(string role) => _httpContextAccessor.HttpContext?.User?.IsInRole(role) == true;
}