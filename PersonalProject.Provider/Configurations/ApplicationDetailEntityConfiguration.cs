using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Configurations;
public class ApplicationDetailEntityConfiguration : IEntityTypeConfiguration<ApplicationDetail>
{
    public void Configure(EntityTypeBuilder<ApplicationDetail> builder)
    {
        builder.Property(b => b.PropertyOwnerEmail)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.LastUpdatedBy)
            .HasMaxLength(255);
    }
}