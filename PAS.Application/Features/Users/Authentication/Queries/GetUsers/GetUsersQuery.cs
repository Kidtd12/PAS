using Application.Common.Security;
using Application.Features.Users.Authentication.Dtos;
using MediatR;

namespace Application.Features.Users.Authentication.Queries;

[Authorize(Permissions = Permissions.Users.View)]
public record GetUsersQuery : IRequest<Result<PaginatedList<UserDto>>>
{
    public string? SearchTerm { get; init; }
    public Guid? RoleId { get; init; }
    public bool? IsActive { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<PaginatedList<UserDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedList<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.UserLogins
            .Include(u => u.Role)
            .Where(u => !u.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(u => u.Username.Contains(request.SearchTerm) || (u.Email ?? string.Empty).Contains(request.SearchTerm));

        if (request.RoleId.HasValue)
            query = query.Where(u => u.RoleId == request.RoleId.Value);

        if (request.IsActive.HasValue)
            query = query.Where(u => u.IsActive == request.IsActive.Value);

        var projected = query.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email ?? string.Empty,
            IsActive = u.IsActive,
            RoleId = u.RoleId,
            RoleName = u.Role != null ? u.Role.RoleName : string.Empty
        });

        var page = await projected.OrderBy(x => x.Username).PaginatedListAsync(request.PageNumber, request.PageSize);
        return Result<PaginatedList<UserDto>>.Success(page);
    }
}
