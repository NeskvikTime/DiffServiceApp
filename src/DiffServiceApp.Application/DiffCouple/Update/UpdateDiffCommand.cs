using MediatR;

namespace DiffServiceApp.Application.DiffCouple.Update;
public record UpdateDiffCommand(int Id, string Data, string Side) : IRequest<Unit>;
