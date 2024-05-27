using NUnit.Framework;

namespace Tests.DocumentsAPI.IntergrationTests;

public class DocumentsIntergrationTestBase
{
    #pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
    private DocumentsWebApplicationFactory? _documentsWebApplicationFactory;
    #pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
    #pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
    protected HttpClient HttpClient;
    #pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
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
