using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Data.Configurations;
public class AddressEntityConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(b => b.Postcode)
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(b => b.AddressLine1)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.AddressLine2)
            .HasMaxLength(255);

        builder.Property(b => b.AddressLine3)
            .HasMaxLength(127);
    }
}