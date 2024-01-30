using DiffServiceApp.Contracts;
using DiffServiceApp.Contracts.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiffServiceApp.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ValidationException validationException:

                var failure = validationException.Errors.FirstOrDefault(exception => exception.ErrorCode == ErrorCodes.NotFound);

                // If there is a NotFound validation error, return 404
                if (failure is not null)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status404NotFound);
                    break;
                }

                // Default case is BadRequest 400
                context.Result = new StatusCodeResult(StatusCodes.Status400BadRequest);
                break;

            case NotFoundException _:
                context.Result = new StatusCodeResult(StatusCodes.Status404NotFound);
                break;

            default:
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                break;
        }
    }
}
