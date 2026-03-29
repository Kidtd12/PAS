using Application.Common.Interfaces;
using Application.Features.Users.Authentication.Commands;
using Application.Features.Users.Authentication.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAS.API.Models.Requests;
using PAS.API.Models.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace PAS.API.Controllers.Auth;

[AllowAnonymous]
public class AuthController : BaseApiController
{
    private readonly ILogger<AuthController> _logger;
    private readonly IApplicationDbContext _context;

    public AuthController(ILogger<AuthController> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// Authenticate user and get JWT token
    /// </summary>
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login to the system")]
    [ProducesResponseType(typeof(ApiResponse<AuthResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResultDto>>> Login(PAS.API.Models.Requests.LoginRequest request)
    {
        _logger.LogInformation("Login attempt for user: {Username}", request.Username);

        var command = new LoginUserCommand
        {
            Username = request.Username,
            Password = request.Password,
            RememberMe = false
        };

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register a new user")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<Guid>>> Register(PAS.API.Models.Requests.RegisterRequest request)
    {
        var employee = await _context.Employees
            .AsNoTracking()
            .Where(e => e.EmployeeCode == request.EmployeeCode && !e.IsDeleted)
            .Select(e => new { e.Id })
            .FirstOrDefaultAsync();

        if (employee == null)
            return BadRequest(ApiResponse<Guid>.ErrorResponse("Employee not found for provided employee code.", 400));

        var command = new RegisterUserCommand
        {
            Username = request.Username,
            Password = request.Password,
            Email = request.Email ?? string.Empty,
            EmployeeId = employee.Id,
            RoleId = request.RoleId
        };

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Logout current user
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    [SwaggerOperation(Summary = "Logout current user")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        var command = new LogoutUserCommand();
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Refresh JWT token
    /// </summary>
    [HttpPost("refresh-token")]
    [SwaggerOperation(Summary = "Refresh JWT token")]
    [ProducesResponseType(typeof(ApiResponse<AuthResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthResultDto>>> RefreshToken([FromBody] PAS.API.Models.Requests.RefreshTokenRequest request)
    {
        var command = new RefreshTokenCommand { RefreshToken = request.RefreshToken };
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Change password
    /// </summary>
    [HttpPost("change-password")]
    [Authorize]
    [SwaggerOperation(Summary = "Change user password")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword(PAS.API.Models.Requests.ChangePasswordRequest request)
    {
        var command = new ChangePasswordCommand
        {
            CurrentPassword = request.CurrentPassword,
            NewPassword = request.NewPassword
        };

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Forgot password - send reset email
    /// </summary>
    [HttpPost("forgot-password")]
    [SwaggerOperation(Summary = "Send password reset email")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword(PAS.API.Models.Requests.ForgotPasswordRequest request)
    {
        var command = new ForgotPasswordCommand { Email = request.Email };
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Reset password with token
    /// </summary>
    [HttpPost("reset-password")]
    [SwaggerOperation(Summary = "Reset password using token")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(PAS.API.Models.Requests.ResetPasswordRequest request)
    {
        var command = new ResetPasswordCommand
        {
            Token = request.Token,
            NewPassword = request.NewPassword
        };

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}