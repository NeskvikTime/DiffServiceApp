using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Domain.DiffPayloadAggregate;

namespace DiffServiceApp.Infrastructure.Persistance.Repository;
internal class DiffPayloadsRepository(ApplicationDbContext _dbContext) : IDiffPayloadsRepository
{
    public async Task AddDiffPayloadAsync(DiffPayload diffPayload, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteDiffPayloadAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DiffPayloadExistsAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<DiffPayload> GetDiffPayloadAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> HasBothValuesAssignedAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateDiffPayloadAsync(DiffPayload diffPayload, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
