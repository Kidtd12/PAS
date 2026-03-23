using MediatR;
using PAS.Application.Features.Users.Authentication.Commands.RegisterUser;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Users.Authentication.Commands;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMediator _mediator;

    public RegisterUserCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IMediator mediator)
    {
        _context = context;
        _currentUser = currentUser;
        _mediator = mediator;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Check if username already exists
        var existingUsername = await _context.UserLogins
            .AnyAsync(u => u.Username == request.Username && !u.IsDeleted, cancellationToken);

        if (existingUsername)
        {
            return Result<Guid>.Failure($"Username '{request.Username}' is already taken.");
        }

        // Check if email already exists
        var existingEmail = await _context.UserLogins
            .AnyAsync(u => u.Email == request.Email && !u.IsDeleted, cancellationToken);

        if (existingEmail)
        {
            return Result<Guid>.Failure($"Email '{request.Email}' is already registered.");
        }

        // Validate employee exists
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == request.EmployeeId && !e.IsDeleted, cancellationToken);

        if (employee == null)
        {
            return Result<Guid>.Failure($"Employee with ID {request.EmployeeId} not found.");
        }

        // Check if employee already has a user account
        var existingEmployeeUser = await _context.UserLogins
            .AnyAsync(u => u.EmployeeId == request.EmployeeId && !u.IsDeleted, cancellationToken);

        if (existingEmployeeUser)
        {
            return Result<Guid>.Failure("This employee already has a user account.");
        }

        // Validate role exists
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == request.RoleId && !r.IsDeleted, cancellationToken);

        if (role == null)
        {
            return Result<Guid>.Failure($"Role with ID {request.RoleId} not found.");
        }

        // Hash password (simplified - use proper hashing in production)
        var passwordHash = HashPassword(request.Password);

        // Create user
        var user = new Domain.Users.UserLogin(
            request.EmployeeId,
            request.Username,
            passwordHash,
            request.RoleId);

        typeof(Domain.Users.UserLogin).GetProperty("Email")?.SetValue(user, request.Email);
        typeof(Domain.Users.UserLogin).GetProperty("IsActive")?.SetValue(user, true);

        _context.UserLogins.Add(user);

        // Create audit trail
        var auditTrail = new AuditTrail(
            _currentUser.UserGuid ?? Guid.Empty,
            "CREATE",
            nameof(Domain.Users.UserLogin),
            user.Id);
        _context.AuditTrails.Add(auditTrail);

        await _context.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new EntityCreatedEvent<Domain.Users.UserLogin>(user, _currentUser.UserGuid), cancellationToken);

        return Result<Guid>.Success(user.Id);
    }

    private string HashPassword(string password)
    {
        // In production, use a proper password hashing algorithm like BCrypt or PBKDF2
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}