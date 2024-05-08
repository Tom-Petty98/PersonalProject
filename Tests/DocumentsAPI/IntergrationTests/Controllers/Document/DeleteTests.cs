using FluentAssertions;
using NUnit.Framework;
using System.Net;

namespace Tests.DocumentsAPI.IntergrationTests.Controllers.Document;

public class DeleteTests : DocumentsIntergrationTestBase
{
    [Test]
    public async Task Delete_Success_NoContent()
    {
        var getResult = await HttpClient.GetAsync($"{DocumentControllerBaseUrl}/{TestDocumentId}");
        getResult.StatusCode.Should().Be(HttpStatusCode.OK);      

        var deleteResult = await HttpClient.DeleteAsync($"{DocumentControllerBaseUrl}/{TestDocumentId}");
        deleteResult.StatusCode.Should().Be(HttpStatusCode.NoContent);

        getResult = await HttpClient.GetAsync($"{DocumentControllerBaseUrl}/{TestDocumentId}");
        getResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Delete_Not_Found_NoContent()
    {
        var testDocumentId = Guid.NewGuid();

        var deleteResult = await HttpClient.DeleteAsync($"{DocumentControllerBaseUrl}/{testDocumentId}");
        deleteResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
