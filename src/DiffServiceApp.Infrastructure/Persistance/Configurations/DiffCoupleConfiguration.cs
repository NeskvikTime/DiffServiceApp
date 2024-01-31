using DiffServiceApp.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiffServiceApp.Infrastructure.Persistance.Configurations;
public class DiffCoupleConfiguration : IEntityTypeConfiguration<DiffPayloadCouple>
{
    public void Configure(EntityTypeBuilder<DiffPayloadCouple> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.Id)
            .IsUnique();

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(e => e.LeftPayloadValue)
            .IsRequired(false);

        builder.Property(e => e.RightPayloadValue)
            .IsRequired(false);
    }
}