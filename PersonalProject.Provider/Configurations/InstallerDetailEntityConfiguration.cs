using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Configurations;
public class InstallerDetailEntityConfiguration : IEntityTypeConfiguration<InstallerDetail>
{
    public void Configure(EntityTypeBuilder<InstallerDetail> builder)
    {
        builder.Property(b => b.InstallerName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.LastUpdatedBy)
            .HasMaxLength(255);
    }
}