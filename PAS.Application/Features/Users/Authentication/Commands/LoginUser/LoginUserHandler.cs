using Application.Features.Users.Authentication.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Users.Authentication.Commands;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthResultDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginUserCommandHandler> _logger;

    public LoginUserCommandHandler(
        IApplicationDbContext context,
        IConfiguration configuration,
        ILogger<LoginUserCommandHandler> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Result<AuthResultDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.UserLogins
            .Include(u => u.Employee)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == request.Username && !u.IsDeleted, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("Login attempt with non-existent username: {Username}", request.Username);
            return Result<AuthResultDto>.Failure("Invalid username or password.");
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Login attempt for inactive user: {Username}", request.Username);
            return Result<AuthResultDto>.Failure("User account is deactivated.");
        }

        // Verify password
        var passwordHash = HashPassword(request.Password);
        if (user.PasswordHash != passwordHash)
        {
            _logger.LogWarning("Invalid password attempt for user: {Username}", request.Username);
            return Result<AuthResultDto>.Failure("Invalid username or password.");
        }

        // Generate JWT token
        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddHours(request.RememberMe ? 168 : 8); // 7 days or 8 hours

        // Update last login
        typeof(Domain.Users.UserLogin).GetProperty("LastLoginAt")?.SetValue(user, DateTime.UtcNow);
        await _context.SaveChangesAsync(cancellationToken);

        // Get user permissions based on role
        var permissions = GetPermissionsForRole(user.Role?.RoleName ?? "Staff");

        var result = new AuthResultDto
        {
            Succeeded = true,
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            User = new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.Employee?.FullName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                EmployeeCode = user.Employee?.EmployeeCode ?? string.Empty,
                Department = user.Employee?.Department ?? string.Empty,
                Roles = new[] { user.Role?.RoleName ?? "User" },
                Permissions = permissions
            }
        };

        return Result<AuthResultDto>.Success(result);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private string GenerateJwtToken(Domain.Users.UserLogin user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim("employeeId", user.EmployeeId.ToString()),
            new Claim("employeeCode", user.Employee?.EmployeeCode ?? string.Empty),
            new Claim("fullName", user.Employee?.FullName ?? string.Empty),
            new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"] ?? "YourSuperSecretKeyForJWTTokenGenerationThatShouldBeAtLeast32BytesLong"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(8);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "PropertyManagementSystem",
            audience: _configuration["Jwt:Audience"] ?? "PropertyManagementClient",
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string[] GetPermissionsForRole(string role)
    {
        return role switch
        {
            SystemRoles.Admin => SystemRoles.RolePermissions.AdminPermissions,
            SystemRoles.StoreOfficer => SystemRoles.RolePermissions.StoreOfficerPermissions,
            SystemRoles.Staff => SystemRoles.RolePermissions.StaffPermissions,
            SystemRoles.Inspector => SystemRoles.RolePermissions.InspectorPermissions,
            SystemRoles.Approver => SystemRoles.RolePermissions.ApproverPermissions,
            SystemRoles.Manager => SystemRoles.RolePermissions.ManagerPermissions,
            _ => Array.Empty<string>()
        };
    }
}