using DiffServiceApp.Contracts.Responses;
using MediatR;

namespace DiffServiceApp.Application.DiffCouple.Queries.GetResult;
public record GetDiffResultQuery(int Id) : IRequest<GetResultResponse>;
