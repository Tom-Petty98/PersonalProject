using NUnit.Framework;

namespace Tests.DocumentsAPI.IntergrationTests;

public class DocumentsIntergrationTestBase
{
    private DocumentsWebApplicationFactory? _documentsWebApplicationFactory;
    protected HttpClient HttpClient;
    protected const string DocumentControllerBaseUrl = "https://localhost:44300/Document";
    protected Guid TestDocumentId = new Guid("3d8c7807-50c9-4ec9-bcee-e6ae77f829ca");

    public DocumentsIntergrationTestBase()
    {
        _documentsWebApplicationFactory = new DocumentsWebApplicationFactory();
        HttpClient = _documentsWebApplicationFactory.CreateClient();
    }

    [SetUp]
    public void SetUp()
    {
        _documentsWebApplicationFactory = new DocumentsWebApplicationFactory();
        HttpClient = _documentsWebApplicationFactory.CreateClient();
    }
}
