using System.Text.Json;
using Application.Core;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware;

public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IHostEnvironment env) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            // This will call the next middleware in the pipeline if request succeeds
            await next(context);
        }
        catch (ValidationException e)
        {
            // This will handle the exception if it is a validation exception
            await HandleValidationException(context, e);
        }
        catch (Exception e)
        {
            // this will handle all other exceptions
            await HandelException(context, e);
        }
    }
    private static async Task HandleValidationException(HttpContext context, ValidationException e)
    {
        var validationErrors = new Dictionary<string, string[]>();

        if (e.Errors is not null)
        {
            foreach (var error in e.Errors)
            {
                if (validationErrors.TryGetValue(error.PropertyName, out var existingErrors))
                {
                    validationErrors[error.PropertyName] = existingErrors.Append(error.ErrorMessage).ToArray();
                }
                else
                {
                    validationErrors[error.PropertyName] = [ error.ErrorMessage ];
                }
            }
        }
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        var validationProblemDetails = new ValidationProblemDetails(validationErrors)
        {
            Type = "ValidationFailure",
            Title = "One or more validation errors occurred",
            Status = StatusCodes.Status400BadRequest,
            Detail = "See the errors property for details",
        };
        
        await context.Response.WriteAsJsonAsync(validationProblemDetails);
    }
    
    private async Task HandelException(HttpContext context, Exception e)
    {
        logger.LogError(e, e.Message);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        var response = env.IsDevelopment()
            ? new AppException(context.Response.StatusCode, e.Message, e.StackTrace)
            : new AppException(context.Response.StatusCode, e.Message, null);
        
        var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        
        var json = JsonSerializer.Serialize(response, options);
        
        await context.Response.WriteAsync(json);
    }
}