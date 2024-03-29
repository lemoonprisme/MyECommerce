using System.Diagnostics;
using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace MyECommerce.Api.ProblemDetails;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ValidationException validationException)
        {
            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsJsonAsync(new Microsoft.AspNetCore.Mvc.ProblemDetails()
            {
                Status = 400,
                Title = "Validation Failed",
                Detail = validationException.Message,
                Extensions =
                {
                    { "traceId", Activity.Current?.Id ?? httpContext.TraceIdentifier }
                }
            }, cancellationToken);
            
            return true;
        }

        return false;
    }
}