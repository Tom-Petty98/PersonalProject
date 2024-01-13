using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Configurations;
public class UserInviteStatusEntityConfiguration : IEntityTypeConfiguration<UserInviteStatus>
{
    public void Configure(EntityTypeBuilder<UserInviteStatus> builder)
    {
        builder.Property(b => b.Description)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.Code)
            .HasConversion<string>()
            .IsUnicode()
            .HasMaxLength(31)
            .IsRequired();
    }
}