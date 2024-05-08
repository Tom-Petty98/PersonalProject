using Azure;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace DocumentsAPI.Services;

public interface IAzureBlobStorageService
{
    Task<string> UploadDocumentAsync(IFormFile document, Guid documentId);
    Task<IFormFile> DownloadDocumentAsync(Guid documentId, string documentName);
    Task DeleteDocumentAsync(Guid documentId);
}

public class AzureBlobStorageService : IAzureBlobStorageService
{
    private readonly object _accountName;
    private readonly string _containerName;

    public AzureBlobStorageService(IConfiguration configuration)
    {
        _accountName = configuration["AzureBlobStorage:AccountName"] ?? throw new ArgumentNullException(nameof(configuration));
        _containerName = configuration["AzureBlobStorage:ContainerName"] ?? throw new ArgumentNullException(nameof(configuration));
    }

    private Uri ContainerUri => new($"https://{_accountName}.blob.core.window.net/{_containerName}");
    private string BlobUriString(Guid id) => $"https://{_accountName}.blob.core.windows.net/{_containerName}/{id}";

    public async Task<string> UploadDocumentAsync(IFormFile document, Guid documentId)
    {
        BlobContainerClient containerClient = new(ContainerUri, new DefaultAzureCredential());
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(documentId.ToString());
        var validationOptions = new UploadTransferValidationOptions
        {
            ChecksumAlgorithm = StorageChecksumAlgorithm.Auto
        };

        var uploadOptions = new BlobUploadOptions()
        {
            TransferValidation = validationOptions
        };

        await blobClient.UploadAsync(document.OpenReadStream(), uploadOptions);
        return BlobUriString(documentId);
    }
    
    public async Task<IFormFile> DownloadDocumentAsync(Guid documentId, string documentName)
    {
        BlobContainerClient containerClient = new(ContainerUri, new DefaultAzureCredential());

        var blobClient = containerClient.GetBlobClient(documentId.ToString());

        MemoryStream stream = new();
        var blobContent = await blobClient.DownloadToAsync(stream);

        IFormFile document = new FormFile(stream, 0, stream.Length, documentName, documentName)
        {
            Headers = new HeaderDictionary(),
            ContentType = blobContent.Headers.ContentType!
        };
        return document;
    }
    public async Task DeleteDocumentAsync(Guid documentId)
    {
        BlobContainerClient containerClient = new(ContainerUri, new DefaultAzureCredential());

        var blobClient = containerClient.GetBlobClient(documentId.ToString());

        try
        {
            await blobClient.DeleteAsync();
        }
        catch (RequestFailedException ex)
        {
            if (ex.Status != 400)
            {
                throw;
            }
        }
    }
}
