using DocumentsAPI.Models;
using DocumentsAPI.Providers.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DocumentsAPI.Providers;

public class DocumentsDbContext : DbContext
{
    public DbSet<Document> Documents { get; set; }

    public DocumentsDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentEntityTypeConfiguration).Assembly);
    }
}
