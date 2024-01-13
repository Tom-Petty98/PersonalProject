using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalProject.Domain.Entities;
using System.Reflection.Emit;

namespace PersonalProject.Provider.Configurations;
public class GlobalSettingsEntityConfiguration : IEntityTypeConfiguration<GlobalSettings>
{
    public void Configure(EntityTypeBuilder<GlobalSettings> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasData(
            new GlobalSettings { Id = 1, NextAppNumber = 10000000, NextInstallerNumber = 10000000 });
    }
}