using Application.Common.Exceptions;
using PAS.API.Models.Responses;
using PAS.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace PAS.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message = "An error occurred while processing your request."
        };

        switch (exception)
        {
            case NotFoundException notFound:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = notFound.Message;
                break;

            case ValidationException validation:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = "Validation failed";
                errorResponse.ValidationErrors = validation.Errors
                    .ToDictionary(k => k.Key, v => v.Value.ToArray());
                break;

            case ForbiddenAccessException:
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = "You do not have permission to access this resource.";
                break;

            case BusinessRuleException businessRule:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = businessRule.Message;
                break;

            case UnauthorizedAccessException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = "Unauthorized access.";
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = _env.IsDevelopment()
                    ? exception.Message
                    : "An internal error occurred.";
                if (_env.IsDevelopment())
                {
                    errorResponse.StackTrace = exception.StackTrace;
                }
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(jsonResponse);
    }
}