using DocumentsAPI.Services;
using Microsoft.AspNetCore.Http;

namespace Tests.DocumentsAPI.IntergrationTests.Mocks;

public class AzureBlobStorageServiceMock : IAzureBlobStorageService
{
    private const string AccountName = "test-account-name";
    private const string ContainerName = "test-container-name";

    public AzureBlobStorageServiceMock()
    {       
    }

    private static string BlobUriString(Guid id) => $"https://{AccountName}.blob.core.windows.net/{ContainerName}/{id}";

    public async Task<string> UploadDocumentAsync(IFormFile document, Guid documentId)
    {
        return await Task.Run(() => BlobUriString(documentId));
    }

    public async Task<IFormFile> DownloadDocumentAsync(Guid documentId, string documentName)
    {
        if (documentId == Guid.Empty) throw new ApplicationException("Not Found on Azure");
        if (documentId == new Guid("a396bc4b-3cc9-414a-9322-d90aa68a4fd8"))
        {
            throw new ApplicationException("Unhandled Exception");
        }

        const string content = "TEST";
        const string contentType = "application/pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        stream.Position = 0;

        IFormFile document = (new FormFile(stream, 0, stream.Length, documentName, documentName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        });

        return document;
    }

    public async Task DeleteDocumentAsync(Guid documentId)
    {
        await Task.Run(() => { });
    }
}
