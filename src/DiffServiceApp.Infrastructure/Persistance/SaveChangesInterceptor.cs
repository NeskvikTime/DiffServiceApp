using DiffServiceApp.Domain.Common;
using DiffServiceApp.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DiffServiceApp.Infrastructure.Persistance;

sealed class SaveChangesInterceptor(IDateTimeProvider _dateTimeProvider) : ISaveChangesInterceptor
{
    public InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        var _context = eventData.Context as ApplicationDbContext;

        if (_context is null)
        {
            throw new ArgumentNullException(nameof(ApplicationDbContext));
        }

        DateTime savingDateTime = _dateTimeProvider.UtcNow;

        UpdateEntityDates(_context.ChangeTracker.Entries<BaseEntity>(), savingDateTime);

        return result;
    }

    public ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return ValueTask.FromResult(SavingChanges(eventData, result));
    }

    private void UpdateEntityDates(IEnumerable<EntityEntry<BaseEntity>> entries, DateTime savingDateTime)
    {
        foreach (var entry in entries.Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.DateModified = savingDateTime;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = savingDateTime;
            }
        }
    }
}
