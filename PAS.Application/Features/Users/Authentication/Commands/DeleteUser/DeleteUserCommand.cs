using Application.Common.Security;
using MediatR;

namespace Application.Features.Users.Authentication.Commands;

[Authorize(Permissions = Permissions.Users.Delete)]
public record DeleteUserCommand(Guid Id) : IRequest<Result>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.UserLogins.FirstOrDefaultAsync(u => u.Id == request.Id && !u.IsDeleted, cancellationToken);
        if (user == null) return Result.Failure("User not found.");

        user.SoftDelete();
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
