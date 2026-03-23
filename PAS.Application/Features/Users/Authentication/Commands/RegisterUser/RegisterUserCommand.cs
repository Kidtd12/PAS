using Application.Common.Security;
using Application.Events;
using Application.Features.Users.Authentication.Dtos;
using FluentValidation;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Users.Authentication.Commands;

public record RegisterUserCommand : IRequest<Result<Guid>>
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public Guid EmployeeId { get; init; }
    public Guid RoleId { get; init; }
}

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(v => v.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters.")
            .Matches("^[a-zA-Z0-9._-]+$").WithMessage("Username can only contain letters, numbers, dots, underscores and hyphens.");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Valid email is required.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

        RuleFor(v => v.EmployeeId)
            .NotEmpty().WithMessage("Employee is required.");

        RuleFor(v => v.RoleId)
            .NotEmpty().WithMessage("Role is required.");
    }
}