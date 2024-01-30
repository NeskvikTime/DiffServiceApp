using DiffServiceApp.Application.Common.Interfaces;
using DiffServiceApp.Domain.DiffPayloadAggregate;
using Microsoft.EntityFrameworkCore;

namespace DiffServiceApp.Infrastructure.Persistance;

internal class ApplicationDbContext : DbContext, IDbInitializer
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<DiffPayload> DiffPayloads => Set<DiffPayload>();

    public void Migrate()
    {
        base.Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
