using Application.Common.Security;
using MediatR;

namespace Application.Features.Users.Roles.Commands.DeleteRole;

[Authorize(Permissions = Permissions.Roles.View)]
public record DeleteRoleCommand(Guid Id) : IRequest<Result>;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);
        if (role == null) return Result.Failure("Role not found.");

        if (await _context.UserLogins.AnyAsync(u => u.RoleId == request.Id && !u.IsDeleted, cancellationToken))
            return Result.Failure("Cannot delete role assigned to users.");

        role.SoftDelete();
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
