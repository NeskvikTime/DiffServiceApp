using DiffServiceApp.Domain.Aggregates;

namespace DiffServiceApp.Application.Common.Interfaces;

public interface IDiffCouplesRepository
{
    Task<DiffPayloadCouple?> GetDiffCoupleAsync(int id, CancellationToken cancellationToken);

    Task CreateDiffCoupleAsync(DiffPayloadCouple diffPayload, CancellationToken cancellationToken);

    Task UpdateDiffPayloadAsync(DiffPayloadCouple diffPayload, CancellationToken cancellationToken);

    Task<bool> DiffCoupleExistsAsync(int id, CancellationToken cancellationToken);

    Task<bool> HasBothValuesAssignedAsync(int id, CancellationToken cancellationToken);

    Task RemoveDiffPayloadCoupleAsync(DiffPayloadCouple diffPayload, CancellationToken cancellationToken);
}
