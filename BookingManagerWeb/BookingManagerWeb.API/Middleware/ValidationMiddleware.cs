using FluentValidation;

namespace BookingManagerWeb.Middleware;

public class ValidationMiddleware : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        foreach (var arg in context.Arguments)
        {
            if (arg is null)
            {
                continue;
            }

            var argumentType =  arg.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator valid)
            {
                continue;
            }

            var valContext = new ValidationContext<object>(arg);
            
            var validationResult = await valid.ValidateAsync(valContext);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }
        }
        
        return await next(context);
    }
}