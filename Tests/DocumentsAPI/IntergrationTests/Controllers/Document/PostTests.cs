using FluentAssertions;
using NUnit.Framework;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;

namespace Tests.DocumentsAPI.IntergrationTests.Controllers.Document;

public class PostTests : DocumentsIntergrationTestBase
{
    [Test]
    public async Task Post_Success_Created()
    {
        const string content = "Test";
        const string fileName = "test.pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        stream.Position = 0;

        var multipartContent = new MultipartFormDataContent
        {
            {
                new StreamContent(stream),
                "file",
                fileName
            }
        };

        var postResult = await HttpClient.PostAsync($"{DocumentControllerBaseUrl}", multipartContent);
        var testDocumentId = Guid.Parse((await postResult.Content.ReadAsStringAsync()).Replace("\"", ""));

        postResult.StatusCode.Should().Be(HttpStatusCode.Created);
        postResult.Headers!.Location?.ToString().Should().Contain($"/Document/{testDocumentId}");

        var getResult = await HttpClient.GetAsync($"{DocumentControllerBaseUrl}/{testDocumentId}");
        getResult.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task PostWithExpiry_Success_Created()
    {
        const string content = "Test";
        const string fileName = "test.pdf";
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
        var testDocumentId = Guid.Parse((await postResult.Content.ReadAsStringAsync()).Replace("\"", ""));

        postResult.StatusCode.Should().Be(HttpStatusCode.Created);
        postResult.Headers!.Location?.ToString().Should().Contain($"/Document/{testDocumentId}");

        var getResult = await HttpClient.GetAsync($"{DocumentControllerBaseUrl}/{testDocumentId}");
        getResult.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
