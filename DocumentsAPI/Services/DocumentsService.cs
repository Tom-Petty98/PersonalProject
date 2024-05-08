using DocumentsAPI.Models;
using DocumentsAPI.Providers;

namespace DocumentsAPI.Services;

public interface IDocumentsService
{
    Task<Guid> UploadDocumentAsync(IFormFile document, DateTime? expiryDate = null);
    Task<IFormFile> DownloadDocumentAsync(Guid documentId);
    Task RemoveExpiryDateAsync(List<Guid> documentIds);
    Task DeleteDocumentAsync(Guid documentId);
    Task DeleteExpiredDocuments();
}

public class DocumentsService : IDocumentsService
{
    private readonly IDocumentsProvider _documentsProvider;
    private readonly IAzureBlobStorageService _azureBlobStorageService;

    public DocumentsService(IDocumentsProvider documentsProvider, IAzureBlobStorageService azureBlobStorageService)
    {
        _documentsProvider = documentsProvider;
        _azureBlobStorageService = azureBlobStorageService;
    }

    public async Task<Guid> UploadDocumentAsync(IFormFile document, DateTime? expiryDate = null)
    {
        Document entity = new()
        {
            FileName = document.FileName,
            CreatedDateTime = DateTime.UtcNow,
            MimeType = document.ContentType,
            ExpiryDateTime = expiryDate,
        };
        entity = await _documentsProvider.AddDocumentAsync(entity);
        var uri = await _azureBlobStorageService.UploadDocumentAsync(document, entity.Id);
        await _documentsProvider.AddAzureUriAsync(entity.Id, uri);

        return entity.Id;
    }

    public async Task<IFormFile> DownloadDocumentAsync(Guid documentId)
    {
        var document = await _documentsProvider.GetDocumentAsync(documentId)
            ?? throw new FileNotFoundException();

        var formFile = await _azureBlobStorageService.DownloadDocumentAsync(documentId, document.FileName);
        return formFile;
    }

    public async Task RemoveExpiryDateAsync(List<Guid> documentIds)
    {
        foreach (var id in documentIds)
        {
            await _documentsProvider.RemoveExpiryDateAsync(id);
        }
    }  

    public async Task DeleteDocumentAsync(Guid documentId)
    {
        await _azureBlobStorageService.DeleteDocumentAsync(documentId);

        await _documentsProvider.MarkAsDeletedAsync(documentId);
    }

    public async Task DeleteExpiredDocuments()
    {
        var expiredDocumentIds = await _documentsProvider.GetExpiredDocumentListAsync();
        foreach (var id in expiredDocumentIds)
        {
            await _azureBlobStorageService.DeleteDocumentAsync(id);
            await _documentsProvider.MarkAsDeletedAsync(id);
        }
    }
}
