using DiffServiceApp.Application.Extensions;
using DiffServiceApp.Domain.Models;
using FluentValidation;

namespace DiffServiceApp.Application.DiffCouple.Update;

sealed class UpdateDiffCommandValidator : AbstractValidator<UpdateDiffCommand>
{
    public UpdateDiffCommandValidator()
    {
        RuleFor(x => x.Data)
            .NotEmpty()
            .Must(data => data.IsValidBase64String())
            .WithMessage("Data must be a valid Base64 string.");


        RuleFor(x => x.Side)
            .NotEmpty()
            .Must(side => string.Equals(nameof(DiffDirection.Left), side, StringComparison.OrdinalIgnoreCase)
                || string.Equals(nameof(DiffDirection.Right), side, StringComparison.OrdinalIgnoreCase))
            .WithMessage("Side must be either 'left' or 'right'.");
    }
}
