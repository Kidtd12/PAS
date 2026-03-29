using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Users.Authentication.Commands;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ChangePasswordCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUser.UserId.HasValue)
            return Result.Failure("User not authenticated.");

        var user = await _context.UserLogins.FirstOrDefaultAsync(u => u.Id == _currentUser.UserId.Value && !u.IsDeleted, cancellationToken);
        if (user == null)
            return Result.Failure("User not found.");

        if (user.PasswordHash != Hash(request.CurrentPassword))
            return Result.Failure("Current password is invalid.");

        typeof(Domain.Users.UserLogin).GetProperty(nameof(Domain.Users.UserLogin.PasswordHash))?.SetValue(user, Hash(request.NewPassword));
        user.MarkUpdated();
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static string Hash(string value)
    {
        using var sha = SHA256.Create();
        return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(value)));
    }
}
