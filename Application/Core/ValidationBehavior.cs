using FluentValidation;
using MediatR;

namespace Application.Core;

// This class is a pipeline behavior that will be used to validate the request
// before it reaches the handler.
public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validator is null) return await next();
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        // If the request is not valid, throw a validation exception to ExceptionMiddleware in API layer
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        
        return await next();
    }
}