using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Users.Authentication.Commands;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public ForgotPasswordCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        _ = await _context.UserLogins.AnyAsync(u => u.Email == request.Email && !u.IsDeleted, cancellationToken);
        return Result.Success();
    }
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public ResetPasswordCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.UserLogins.FirstOrDefaultAsync(u => u.Username == request.Token && !u.IsDeleted, cancellationToken);
        if (user == null)
            return Result.Failure("Invalid reset token.");

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
