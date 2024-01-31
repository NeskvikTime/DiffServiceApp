using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Domain.Aggregates;
using DiffServiceApp.Infrastructure.Persistance.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DiffServiceApp.Infrastructure.Persistance;

sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options), IDbInitializer
{
    public DbSet<DiffPayloadCouple> DiffPayloadCouples => Set<DiffPayloadCouple>();

    public void Migrate()
    {
        base.Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DiffCoupleConfiguration());
    }
}
