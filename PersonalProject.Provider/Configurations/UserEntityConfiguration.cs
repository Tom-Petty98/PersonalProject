using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Configurations;
public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(b => b.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.LastUpdatedBy)
            .HasMaxLength(255);
    }
}