using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Contracts.Common;
using DiffServiceApp.Contracts.Exceptions;
using DiffServiceApp.Contracts.Responses;
using DiffServiceApp.Domain.Models;
using MediatR;

namespace DiffServiceApp.Application.DiffCouple.Queries.GetResult;
internal class GetDiffResultQueryHandler(IDiffCouplesRepository _diffCouplesRepository,
    IDiffCoupleProcessor _processor) : IRequestHandler<GetDiffResultQuery, GetResultResponse>
{
    public async Task<GetResultResponse> Handle(GetDiffResultQuery request, CancellationToken cancellationToken)
    {
        var response = new GetResultResponse();

        var diffCouple = await _diffCouplesRepository.GetDiffCoupleAsync(request.Id, cancellationToken);

        if (diffCouple is null)
        {
            throw new NotFoundException($"Diff couple with Id {request.Id} not found.");
        }

        DiffResult result = _processor.GetDiffResult(diffCouple);

        response.Result = result.DiffResultType.ToString();

        if (result.Diffs is not null)
        {
            response.Diffs = result.Diffs.Select(x => new DiffResponse(x.Offset, x.Length)).ToList();
        }

        return response;
    }
}
