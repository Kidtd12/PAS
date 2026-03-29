using Application.Common.Security;
using MediatR;

namespace Application.Features.Users.Authentication.Commands;

[Authorize(Permissions = Permissions.Users.Edit)]
public record UpdateUserCommand : IRequest<Result>
{
    public Guid Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public Guid RoleId { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.UserLogins.FirstOrDefaultAsync(u => u.Id == request.Id && !u.IsDeleted, cancellationToken);
        if (user == null) return Result.Failure("User not found.");

        if (await _context.UserLogins.AnyAsync(u => u.Username == request.Username && u.Id != request.Id && !u.IsDeleted, cancellationToken))
            return Result.Failure("Username already exists.");

        typeof(Domain.Users.UserLogin).GetProperty(nameof(Domain.Users.UserLogin.Username))?.SetValue(user, request.Username);
        typeof(Domain.Users.UserLogin).GetProperty(nameof(Domain.Users.UserLogin.Email))?.SetValue(user, request.Email);
        typeof(Domain.Users.UserLogin).GetProperty(nameof(Domain.Users.UserLogin.RoleId))?.SetValue(user, request.RoleId);
        typeof(Domain.Users.UserLogin).GetProperty(nameof(Domain.Users.UserLogin.IsActive))?.SetValue(user, request.IsActive);
        user.MarkUpdated();

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
