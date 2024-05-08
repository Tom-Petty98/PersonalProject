using FluentAssertions;
using NUnit.Framework;
using System.Globalization;
using System.Net;

namespace Tests.DocumentsAPI.IntergrationTests.Controllers.Document;

public class CleanUpTests : DocumentsIntergrationTestBase
{
    [Test]
    public async Task CleanUp_Success_NoContent()
    {
        const string content = "Test";
        const string fileName = "cleanuptest.pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        stream.Position = 0;
        var expiryDateTime = DateTime.UtcNow.AddDays(-10);
        var multipartContent = new MultipartFormDataContent
        {
            {
                new StreamContent(stream),
                "file",
                fileName
            }
        };

        var postResult = await HttpClient.PostAsync(
            $"{DocumentControllerBaseUrl}/{expiryDateTime.ToString("O", CultureInfo.InvariantCulture)}",
            multipartContent);
        postResult.StatusCode.Should().Be(HttpStatusCode.Created);

        var testDocumentId = Guid.Parse((await postResult.Content.ReadAsStringAsync()).Replace("\"", ""));

        var getResult = await HttpClient.GetAsync($"{DocumentControllerBaseUrl}/{testDocumentId}");
        getResult.StatusCode.Should().Be(HttpStatusCode.OK);      

        var cleanResult = await HttpClient.PostAsync($"{DocumentControllerBaseUrl}/CleanUp", null);
        cleanResult.StatusCode.Should().Be(HttpStatusCode.NoContent);

        getResult = await HttpClient.GetAsync($"{DocumentControllerBaseUrl}/{testDocumentId}");
        getResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
