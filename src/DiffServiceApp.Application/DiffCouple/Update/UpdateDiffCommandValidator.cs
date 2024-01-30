using DiffServiceApp.Application.Extensions;
using DiffServiceApp.Contracts;
using DiffServiceApp.Domain.Models;
using FluentValidation;

namespace DiffServiceApp.Application.DiffCouple.Update;

public class UpdateDiffCommandValidator : AbstractValidator<UpdateDiffCommand>
{
    public UpdateDiffCommandValidator()
    {
        RuleFor(x => x.Data)
            .NotEmpty()
            .Must(data => data.IsValidBase64String())
            .WithErrorCode(ErrorCodes.BadRequest);

        RuleFor(x => x.Side)
            .NotEmpty()
            .Must(side => string.Equals(nameof(DiffSide.Left), side, StringComparison.OrdinalIgnoreCase)
                || string.Equals(nameof(DiffSide.Right), side, StringComparison.OrdinalIgnoreCase))
            .WithErrorCode(ErrorCodes.BadRequest);
    }
}
