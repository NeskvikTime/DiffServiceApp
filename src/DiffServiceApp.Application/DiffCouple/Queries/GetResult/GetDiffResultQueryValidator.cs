using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Contracts.Exceptions;
using FluentValidation;

namespace DiffServiceApp.Application.DiffCouple.Queries.GetResult;
public class GetDiffResultQueryValidator : AbstractValidator<GetDiffResultQuery>
{
    private readonly IDiffCouplesRepository _diffCouplesRepository;

    public GetDiffResultQueryValidator(IDiffCouplesRepository diffCouplesRepository)
    {
        _diffCouplesRepository = diffCouplesRepository;

        RuleFor(x => x.Id)
            .CustomAsync(CheckDiffCoupleExistsAsync)
            .CustomAsync(CheckBothValuesAssignedAsync);
    }

    private async Task CheckDiffCoupleExistsAsync(int id, ValidationContext<GetDiffResultQuery> context, CancellationToken cancellationToken)
    {
        if (!await _diffCouplesRepository.DiffCoupleExistsAsync(id, cancellationToken))
        {
            throw new NotFoundException($"Diff couple with Id: '{id}' not found");
        }
    }

    private async Task CheckBothValuesAssignedAsync(int id, ValidationContext<GetDiffResultQuery> context, CancellationToken cancellationToken)
    {
        if (!await _diffCouplesRepository.HasBothValuesAssignedAsync(id, cancellationToken))
        {
            throw new NotFoundException($"Diff couple with Id: '{id}' not found");
        }
    }
}