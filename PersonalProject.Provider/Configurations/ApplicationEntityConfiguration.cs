using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Configurations;
public class ApplicationEntityConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
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