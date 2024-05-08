using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Headers;

namespace Tests.DocumentsAPI.IntergrationTests.Controllers.Document;

public class AllTests : DocumentsIntergrationTestBase
{
    [Test]
    public async Task All_Success()
    {
        const string content = "Test";
        const string fileName = "test.pdf";
        const string contentType = "application/pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        stream.Position = 0;

        var multipartContent = new MultipartFormDataContent
        {
            {
                new StreamContent(stream)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue(contentType) }
                },
                "file",
                fileName
            }
        };

        var postResult = await HttpClient.PostAsync($"{DocumentControllerBaseUrl}", multipartContent);
        postResult.StatusCode.Should().Be(HttpStatusCode.Created);

        var testDocumentId = Guid.Parse((await postResult.Content.ReadAsStringAsync()).Replace("\"", ""));
        var getResult = await HttpClient.GetAsync($"{DocumentControllerBaseUrl}/{testDocumentId}");

        getResult.StatusCode.Should().Be(HttpStatusCode.OK);
        getResult.Content.Headers.ContentType!.ToString().Should().Be(contentType);
        getResult.Content.Headers.ContentDisposition!.ToString().Should()
            .Be("attachment; filename=test.pdf; filename*=UTF-8''test.pdf");

        var deleteResult = await HttpClient.DeleteAsync($"{DocumentControllerBaseUrl}/{testDocumentId}");
        deleteResult.StatusCode.Should().Be(HttpStatusCode.NoContent);

        getResult = await HttpClient.GetAsync($"{DocumentControllerBaseUrl}/{testDocumentId}");
        getResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
