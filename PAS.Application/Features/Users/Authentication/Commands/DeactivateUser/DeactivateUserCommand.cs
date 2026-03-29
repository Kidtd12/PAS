using Application.Common.Security;
using MediatR;

namespace Application.Features.Users.Authentication.Commands;

[Authorize(Permissions = Permissions.Users.Edit)]
public record DeactivateUserCommand(Guid Id) : IRequest<Result>;

public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeactivateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.UserLogins.FirstOrDefaultAsync(u => u.Id == request.Id && !u.IsDeleted, cancellationToken);
        if (user == null) return Result.Failure("User not found.");

        typeof(Domain.Users.UserLogin).GetProperty(nameof(Domain.Users.UserLogin.IsActive))?.SetValue(user, false);
        user.MarkUpdated();
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
