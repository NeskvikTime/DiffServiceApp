using DiffServiceApp.Domain.DiffPayloadAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiffServiceApp.Infrastructure.Persistance.Configurations;
public class DiffPayloadConfiguration : IEntityTypeConfiguration<DiffPayload>
{
    public void Configure(EntityTypeBuilder<DiffPayload> builder)
    {

    }
}