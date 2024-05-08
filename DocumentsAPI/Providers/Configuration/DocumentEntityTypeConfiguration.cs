using DocumentsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentsAPI.Providers.Configuration;

public class DocumentEntityTypeConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        //builder.Property(d => d.Id).HasDefaultValueSql("NewId()");

        builder.HasIndex(d => d.FileName);

        builder.Property(d => d.MimeType).HasMaxLength(255);
    }
}
