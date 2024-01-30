using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace DiffServiceApp.Infrastructure.Persistance.Repository;
internal class DiffCouplesRepository(ApplicationDbContext _dbContext) : IDiffCouplesRepository
{
    public async Task CreateDiffCoupleAsync(DiffPayloadCouple diffPayload, CancellationToken cancellationToken)
    {
        await _dbContext.DiffPayloadCouples.AddAsync(diffPayload, cancellationToken);
    }

    public async Task<bool> DiffCoupleExistsAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.DiffPayloadCouples
            .AsNoTracking()
            .AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<DiffPayloadCouple?> GetDiffCoupleAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.DiffPayloadCouples
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> HasBothValuesAssignedAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.DiffPayloadCouples
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && x.LeftPayloadValue != null && x.RightPayloadValue != null, cancellationToken);
    }

    public Task UpdateDiffPayloadAsync(DiffPayloadCouple diffPayload, CancellationToken cancellationToken)
    {
        _dbContext.DiffPayloadCouples.Update(diffPayload);

        return Task.CompletedTask;
    }
}
