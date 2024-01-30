using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Contracts;
using FluentValidation;

namespace DiffServiceApp.Application.DiffCouple.Queries.GetResult;
internal class GetDiffResultQueryValidator : AbstractValidator<GetDiffResultQuery>
{
    private readonly IDiffCouplesRepository _diffCouplesRepository;

    public GetDiffResultQueryValidator(IDiffCouplesRepository diffCouplesRepository)
    {
        _diffCouplesRepository = diffCouplesRepository;

        RuleFor(x => x.Id)
            .GreaterThan(0)
            .MustAsync(async (id, cancellationToken) => await _diffCouplesRepository.DiffCoupleExistsAsync(id, cancellationToken))
            .WithErrorCode(ErrorCodes.NotFound)
            .MustAsync(async (id, cancellationToken) => await _diffCouplesRepository.HasBothValuesAssignedAsync(id, cancellationToken))
            .WithErrorCode(ErrorCodes.BadRequest);
    }
}
