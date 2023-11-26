using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Data.Configurations;
public class ApplicationStatusHistoryEntityConfiguration : IEntityTypeConfiguration<ApplicationStatusHistory>
{
    public void Configure(EntityTypeBuilder<ApplicationStatusHistory> builder)
    {
        builder.Property(b => b.StatusChangedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}