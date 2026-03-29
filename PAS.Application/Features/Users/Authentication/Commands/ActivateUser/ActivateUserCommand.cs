using Application.Common.Security;
using MediatR;

namespace Application.Features.Users.Authentication.Commands;

[Authorize(Permissions = Permissions.Users.Edit)]
public record ActivateUserCommand(Guid Id) : IRequest<Result>;

public class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public ActivateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.UserLogins.FirstOrDefaultAsync(u => u.Id == request.Id && !u.IsDeleted, cancellationToken);
        if (user == null) return Result.Failure("User not found.");

        typeof(Domain.Users.UserLogin).GetProperty(nameof(Domain.Users.UserLogin.IsActive))?.SetValue(user, true);
        user.MarkUpdated();
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
