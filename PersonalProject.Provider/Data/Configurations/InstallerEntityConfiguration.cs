using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Data.Configurations;
public class InstallerEntityConfiguration : IEntityTypeConfiguration<Installer>
{
    public void Configure(EntityTypeBuilder<Installer> builder)
    {
        builder.Property(b => b.RefNumber)
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(b => b.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.LastUpdatedBy)
            .HasMaxLength(255);
    }
}