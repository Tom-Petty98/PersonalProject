using Microsoft.AspNetCore.Mvc.ModelBinding;
using PersonalProject.InternalPortal.Services.Helpers;
using Polly.Registry;

namespace PersonalProject.InternalPortal.Services.General;

public interface IDocumentsService
{
    public Task DeleteDocumentById(Guid id);
    public Task<IFormFile?> GetDocumentById(Guid id);
    public Task<Guid> UploadDocument(IFormFile document, TimeSpan? optionalLifeSpan);
    public Task<Guid> UploadTempDocument(IFormFile document);
    public Task RemoveTempDocumentsExpiryDate(List<Guid> documentIds);
    public string? ValidateFile(IFormFile file, string modelStateKey, ModelStateDictionary modelState);
}

public class DocumentsService : BaseRequestsClient<DocumentsService>, IDocumentsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string _clientName = "AI_FAQ_API";

    public DocumentsService(IHttpClientFactory httpClientFactory, ILogger<DocumentsService> logger, IPolicyRegistry<string> pollySettings)
        : base(pollySettings, logger)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient BuildClient() => _httpClientFactory.CreateClient(_clientName);

    public async Task DeleteDocumentById(Guid id)
    {
        var httpClient = BuildClient();
        var target = $"Document/{id}";

        await DeleteAsync<object, object>(httpClient, target, null);
    }

    public async Task<IFormFile?> GetDocumentById(Guid id)
    {
        var httpClient = BuildClient();
        var target = $"Document/{id}";

        var response = await httpClient.GetAsync(target);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        var fileName = response.Content.Headers.ContentDisposition!.FileNameStar ?? string.Empty;
        return new FormFile(
            await response.Content.ReadAsStreamAsync(),
            0,
            response.Content.Headers.ContentLength!.Value,
            fileName,
            fileName)
        { Headers = new HeaderDictionary(), ContentType = response.Content.Headers.ContentType!.ToString() };
    }

    public async Task<Guid> UploadDocument(IFormFile document, TimeSpan? optionalLifeSpan = null)
    {
        var httpClient = BuildClient();
        var target = $"Document";

        if (optionalLifeSpan != null)
        {
            target += "/" + DateTime.UtcNow.Add(optionalLifeSpan.Value).ToString("o");
        }

        var multipartContent = new MultipartFormDataContent
        {
            { new StreamContent(document.OpenReadStream())
                { Headers =
                    { ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue( document.ContentType )}
                },
                document.Name, document.FileName
            }
        };

        return await PostAsync<MultipartFormDataContent, Guid>(httpClient, target, multipartContent);
    }

    public async Task<Guid> UploadTempDocument(IFormFile document)
    {
        return await UploadDocument(document, TimeSpan.FromDays(1));
    }

    public async Task RemoveTempDocumentsExpiryDate(List<Guid> documentIds)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(RemoveTempDocumentsExpiryDate));
        var target = $"RemoveTempDocumentsExpiry";

        await PostAsync<List<Guid>, object>(httpClient, target, documentIds, default, pollyParams);
    }

    public string? ValidateFile(IFormFile file, string modelStateKey, ModelStateDictionary modelState)
    {
        throw new NotImplementedException();
    }
}