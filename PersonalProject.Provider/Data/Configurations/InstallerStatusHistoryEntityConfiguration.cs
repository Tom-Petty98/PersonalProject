using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Data.Configurations;
public class InstallerStatusHistoryEntityConfiguration : IEntityTypeConfiguration<InstallerStatusHistory>
{
    public void Configure(EntityTypeBuilder<InstallerStatusHistory> builder)
    {
        builder.Property(b => b.StatusChangedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}