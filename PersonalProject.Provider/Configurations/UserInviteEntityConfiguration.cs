using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Configurations;
public class UserInviteEntityConfiguration : IEntityTypeConfiguration<UserInvite>
{
    public void Configure(EntityTypeBuilder<UserInvite> builder)
    {
        builder.Property(b => b.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.LastUpdatedBy)
            .HasMaxLength(255);
    }
}