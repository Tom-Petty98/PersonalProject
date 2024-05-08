using DocumentsAPI.Models;
using DocumentsAPI.Providers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tests.DocumentsAPI.IntergrationTests;

public class DocumentsDbContextFactory : IDesignTimeDbContextFactory<DocumentsDbContext>
{
    public DocumentsDbContext CreateDbContext(string[] args)
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.CreateFunction("newid", () => Guid.NewGuid());
        connection.Open();

        var options = new DbContextOptionsBuilder<DocumentsDbContext>()
            .UseSqlite(connection)
            .Options;

        var dbContext = new DocumentsDbContext(options);

        dbContext.Database.EnsureCreated();

        dbContext.Documents.Add(new Document() { AzureStorageUri = "uri", CreatedDateTime = DateTime.UtcNow, FileName = "Test.pdf", Id = Guid.Parse("3d8c7807-50c9-4ec9-bcee-e6ae77f829ca") });
        dbContext.Documents.Add(new Document() { AzureStorageUri = "exception", CreatedDateTime = DateTime.UtcNow, FileName = "Test.pdf", Id = Guid.Parse("a396bc4b-3cc9-414a-9322-d90aa68a4fd8") });

        return dbContext;
    }
}
