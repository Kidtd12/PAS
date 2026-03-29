using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.Identity;

namespace Application.Features.Users.Authentication.Commands;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RegisterUserCommandHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Check if username already exists in Identity
        var existingIdentityUsername = await _userManager.FindByNameAsync(request.Username);
        if (existingIdentityUsername != null)
        {
            return Result<Guid>.Failure($"Username '{request.Username}' is already taken.");
        }

        // Check if email already exists in Identity
        var existingIdentityEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingIdentityEmail != null)
        {
            return Result<Guid>.Failure($"Email '{request.Email}' is already registered.");
        }

        // Validate employee exists
        var employee = await _context.Employees
            .Where(e => e.Id == request.EmployeeId && !e.IsDeleted)
            .Select(e => new { e.Id, e.FullName })
            .FirstOrDefaultAsync(cancellationToken);

        if (employee == null)
        {
            return Result<Guid>.Failure($"Employee with ID {request.EmployeeId} not found.");
        }

        // Validate role exists
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == request.RoleId && !r.IsDeleted, cancellationToken);

        if (role == null || string.IsNullOrWhiteSpace(role.RoleName))
        {
            return Result<Guid>.Failure($"Role with ID {request.RoleId} not found.");
        }

        // Create Identity user in AspNetUsers
        var identityUser = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            FullName = employee.FullName,
            IsActive = true,
            EmailConfirmed = true
        };

        var identityCreateResult = await _userManager.CreateAsync(identityUser, request.Password);
        if (!identityCreateResult.Succeeded)
        {
            var errors = string.Join("; ", identityCreateResult.Errors.Select(e => e.Description));
            return Result<Guid>.Failure($"User registration failed: {errors}");
        }

        if (!await _roleManager.RoleExistsAsync(role.RoleName))
        {
            var createRoleResult = await _roleManager.CreateAsync(new ApplicationRole
            {
                Name = role.RoleName,
                Description = role.Description
            });

            if (!createRoleResult.Succeeded)
            {
                var errors = string.Join("; ", createRoleResult.Errors.Select(e => e.Description));
                return Result<Guid>.Failure($"Role provisioning failed: {errors}");
            }
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(identityUser, role.RoleName);
        if (!addToRoleResult.Succeeded)
        {
            var errors = string.Join("; ", addToRoleResult.Errors.Select(e => e.Description));
            return Result<Guid>.Failure($"Role assignment failed: {errors}");
        }

        return Guid.TryParse(identityUser.Id, out var userId)
            ? Result<Guid>.Success(userId)
            : Result<Guid>.Success(Guid.Empty);
    }
}