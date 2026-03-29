using MediatR;

namespace Application.Features.Users.Authentication.Commands;

public record ForgotPasswordCommand : IRequest<Result>
{
    public string Email { get; init; } = string.Empty;
}

public record ResetPasswordCommand : IRequest<Result>
{
    public string Token { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
}
