using DocumentsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsAPI.Providers;

public interface IDocumentsProvider
{
    Task<Document> AddDocumentAsync(Document document);
    Task<Document?> GetDocumentAsync(Guid documentId);
    Task AddAzureUriAsync(Guid documentId, string uri);
    Task MarkAsDeletedAsync(Guid documentId);
    Task RemoveExpiryDateAsync(Guid documentId);
    Task<List<Guid>> GetExpiredDocumentListAsync();
}
public class DocumentsProvider : IDocumentsProvider
{
    private readonly DocumentsDbContext _dbContext;

    public DocumentsProvider(DocumentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Document> AddDocumentAsync(Document document)
    {
        var created = (await _dbContext.AddAsync(document)).Entity;
        await _dbContext.SaveChangesAsync();
        return created;
    }

    public async Task<Document?> GetDocumentAsync(Guid documentId)
    {
        var document = await _dbContext.Documents!.FindAsync(documentId);
        return document?.DeletedDateTime == null ? document : null;
    }

    public async Task AddAzureUriAsync(Guid documentId, string uri)
    {
        var document = await _dbContext.Documents.FindAsync(documentId);
        if (document == null)
        {
            throw new FileNotFoundException();
        }
        document.AzureStorageUri = uri;
        await _dbContext.SaveChangesAsync();
    }

    public async Task MarkAsDeletedAsync(Guid documentId)
    {
        var document = await _dbContext.Documents.FindAsync(documentId);
        if (document != null)
        {
            document.DeletedDateTime = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }      
    }

    public async Task RemoveExpiryDateAsync(Guid documentId)
    {
        var document = await _dbContext.Documents.FindAsync(documentId);
        if (document != null)
        {
            document.ExpiryDateTime = null!;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<Guid>> GetExpiredDocumentListAsync()
    {
        return await _dbContext.Documents
            .Where(d => d.DeletedDateTime == null && d.ExpiryDateTime != null && d.ExpiryDateTime < DateTime.UtcNow)
            .Select(d => d.Id)
            .ToListAsync();
    }
}
