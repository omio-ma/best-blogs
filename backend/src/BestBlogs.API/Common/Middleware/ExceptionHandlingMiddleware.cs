using BestBlogs.API.Common.Models;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace BestBlogs.API.Common.Middleware;

/// <summary>
/// Global exception handling middleware for consistent error responses
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        var response = context.Response;
        response.ContentType = "application/json";

        var apiResponse = exception switch
        {
            ValidationException validationException => HandleValidationException(validationException, response),
            ArgumentException argumentException => HandleArgumentException(argumentException, response),
            UnauthorizedAccessException => HandleUnauthorizedException(response),
            _ => HandleUnknownException(exception, response)
        };

        var result = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(result);
    }

    private static ApiResponse HandleValidationException(ValidationException exception, HttpResponse response)
    {
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        var errors = exception.Errors.Select(e => e.ErrorMessage).ToList();
        return ApiResponse.ErrorResponse("Validation failed", errors);
    }

    private static ApiResponse HandleArgumentException(ArgumentException exception, HttpResponse response)
    {
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        return ApiResponse.ErrorResponse(exception.Message);
    }

    private static ApiResponse HandleUnauthorizedException(HttpResponse response)
    {
        response.StatusCode = (int)HttpStatusCode.Unauthorized;
        return ApiResponse.ErrorResponse("Unauthorized access");
    }

    private static ApiResponse HandleUnknownException(Exception exception, HttpResponse response)
    {
        response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return ApiResponse.ErrorResponse("An internal server error occurred");
    }
}
