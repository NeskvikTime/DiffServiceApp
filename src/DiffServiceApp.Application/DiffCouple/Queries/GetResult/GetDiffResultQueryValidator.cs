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
            .CustomAsync(async (id, contect, cancellationToken) =>
            {
                if (!await _diffCouplesRepository.DiffCoupleExistsAsync(id, cancellationToken))
                {
                    throw new NotFoundException("Diff couple with  Id: '{PropertyValue}' not found");
                }
            })
            .CustomAsync(async (id, contect, cancellationToken) =>
            {
                if (!await _diffCouplesRepository.HasBothValuesAssignedAsync(id, cancellationToken))
                {
                    throw new NotFoundException("Diff couple with  Id: '{PropertyValue}' not found");
                }
            });
    }
}
