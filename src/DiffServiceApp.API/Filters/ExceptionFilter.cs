using DiffServiceApp.Contracts.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiffServiceApp.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        ProblemDetails? problemDetails = null;

        problemDetails = context.Exception switch
        {
            // FluentValidationException is thrown by FluentValidation
            ValidationException validationException => new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation error",
                Detail = string.Join(" ", validationException.Errors
                                    .Select(e => e.ErrorMessage))
            },

            // BadRequestException is thrown by the application
            NotFoundException _ => new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Status = StatusCodes.Status404NotFound,
                Title = "Not found",
                Detail = context.Exception.Message
            },

            // Other exceptions inside application , than internal server error is returned
            _ => new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal server error",
                Detail = context.Exception?.InnerException?.Message ?? context.Exception?.Message
            },
        };
        context.Result = new JsonResult(problemDetails) { StatusCode = problemDetails!.Status };
    }
}
