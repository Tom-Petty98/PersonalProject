using FluentAssertions;
using NUnit.Framework;
using System.Net;

namespace Tests.DocumentsAPI.IntergrationTests.Controllers.Document;

public class GetTests : DocumentsIntergrationTestBase
{
    [Test]
    public async Task Get_Exists_Ok()
    {
        var getResult = await HttpClient.GetAsync($"{DocumentControllerBaseUrl}/{TestDocumentId}");
        getResult.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task Get_NotExists_404()
    {
        var getResult = await HttpClient.GetAsync($"{DocumentControllerBaseUrl}/{Guid.Empty}");
        getResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Get_Exception_500()
    {
        var getResult = await HttpClient.GetAsync($"{DocumentControllerBaseUrl}/a396bc4b-3cc9-414a-9322-d90aa68a4fd8");
        getResult.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
