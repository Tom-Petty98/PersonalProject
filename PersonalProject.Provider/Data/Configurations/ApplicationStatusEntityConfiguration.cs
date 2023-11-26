﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Data.Configurations;
public class ApplicationStatusEntityConfiguration : IEntityTypeConfiguration<ApplicationStatus>
{
    public void Configure(EntityTypeBuilder<ApplicationStatus> builder)
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