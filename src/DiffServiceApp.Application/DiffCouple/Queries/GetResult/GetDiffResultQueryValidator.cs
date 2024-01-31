using DiffServiceApp.Application.Common.Interfaces;
using FluentValidation;

namespace DiffServiceApp.Application.DiffCouple.Queries.GetResult;
public class GetDiffResultQueryValidator : AbstractValidator<GetDiffResultQuery>
{
    private readonly IDiffCouplesRepository _diffCouplesRepository;

    public GetDiffResultQueryValidator(IDiffCouplesRepository diffCouplesRepository)
    {
        _diffCouplesRepository = diffCouplesRepository;

        RuleFor(x => x.Id)
            .GreaterThan(0)
            .MustAsync(async (id, cancellationToken) => await _diffCouplesRepository.DiffCoupleExistsAsync(id, cancellationToken))
            .WithMessage("Diff couple with Id: '{PropertyValue}' not found.")
            .MustAsync(async (id, cancellationToken) => await _diffCouplesRepository.HasBothValuesAssignedAsync(id, cancellationToken))
            .WithMessage("Diff couple with Id: '{PropertyValue}' does not have both sides assigned.");
    }
}
