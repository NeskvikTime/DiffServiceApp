using FluentValidation;
using MediatR;

namespace DiffServiceApp.Application.Common.Behaviours;
public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator = validator;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        await _validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        return await next();
    }
}

