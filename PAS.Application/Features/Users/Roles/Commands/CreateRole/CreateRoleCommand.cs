using Application.Common.Security;
using MediatR;

namespace Application.Features.Users.Roles.Commands.CreateRole;

[Authorize(Permissions = Permissions.Roles.View)]
public record CreateRoleCommand : IRequest<Result<Guid>>
{
    public string RoleName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Roles.AnyAsync(r => r.RoleName == request.RoleName && !r.IsDeleted, cancellationToken))
            return Result<Guid>.Failure("Role already exists.");

        var role = new Domain.Users.Role(request.RoleName, request.Description);
        _context.Roles.Add(role);
        await _context.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(role.Id);
    }
}
