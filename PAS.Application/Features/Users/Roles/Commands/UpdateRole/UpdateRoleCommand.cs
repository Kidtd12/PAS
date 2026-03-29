using Application.Common.Security;
using MediatR;

namespace Application.Features.Users.Roles.Commands.UpdateRole;

[Authorize(Permissions = Permissions.Roles.View)]
public record UpdateRoleCommand : IRequest<Result>
{
    public Guid Id { get; init; }
    public string RoleName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);
        if (role == null) return Result.Failure("Role not found.");

        typeof(Domain.Users.Role).GetProperty(nameof(Domain.Users.Role.RoleName))?.SetValue(role, request.RoleName);
        typeof(Domain.Users.Role).GetProperty(nameof(Domain.Users.Role.Description))?.SetValue(role, request.Description);
        role.MarkUpdated();
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
