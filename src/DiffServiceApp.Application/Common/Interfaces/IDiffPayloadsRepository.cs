using DiffServiceApp.Domain.DiffPayloadAggregate;

namespace DiffServiceApp.Application.Common.Interfaces;

public interface IDiffPayloadsRepository
{
    Task<DiffPayload> GetDiffPayloadAsync(int id, CancellationToken cancellationToken);

    Task AddDiffPayloadAsync(DiffPayload diffPayload, CancellationToken cancellationToken);

    Task UpdateDiffPayloadAsync(DiffPayload diffPayload, CancellationToken cancellationToken);

    Task DeleteDiffPayloadAsync(int id, CancellationToken cancellationToken);

    Task<bool> DiffPayloadExistsAsync(int id, CancellationToken cancellationToken);

    Task<bool> HasBothValuesAssignedAsync(int id, CancellationToken cancellationToken);
}
