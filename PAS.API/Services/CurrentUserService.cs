using Application.Common.Interfaces;
using Application.Constants;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PAS.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _context;
    private Guid? _resolvedUserId;
    private bool _resolved;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
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

            // 1) If token subject is already legacy UserLogin.Id
            if (Guid.TryParse(subject, out var parsedId))
            {
                var byId = _context.UserLogins
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Id == parsedId && !u.IsDeleted);

                if (byId != null)
                {
                    _resolvedUserId = byId.Id;
                    return _resolvedUserId;
                }
            }

            // 2) Resolve from AspNetUsers.Id stored in UserLogin.AspNetUserId
            var byAspNetUserId = _context.UserLogins
                .AsNoTracking()
                .FirstOrDefault(u => u.AspNetUserId == subject && !u.IsDeleted);

            if (byAspNetUserId != null)
            {
                _resolvedUserId = byAspNetUserId.Id;
                return _resolvedUserId;
            }

            // 3) Final fallback by username
            var username = Username;
            if (!string.IsNullOrWhiteSpace(username))
            {
                var byUsername = _context.UserLogins
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Username == username && !u.IsDeleted);

                _resolvedUserId = byUsername?.Id;
                return _resolvedUserId;
            }

            _resolvedUserId = null;
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
            if (!UserId.HasValue)
                return Enumerable.Empty<string>();

            var user = _context.UserLogins
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Id == UserId && !u.IsDeleted);

            if (user?.Role == null)
                return Enumerable.Empty<string>();

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

    public bool IsAuthenticated => UserId.HasValue;
    public bool IsInRole(string role) => _httpContextAccessor.HttpContext?.User?.IsInRole(role) == true;
}