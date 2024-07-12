using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalProject.Domain.Request;
using Polly.Registry;
using System.Net;
using System.Text.Json;

namespace Tests.ConsentPortal.UnitTests.Services.Extensions;

[TestFixture]
public class BaseClientUnsuccessfulResponseTests
{
    private readonly Mock<ILogger<StubbedRequestsClient>> _mockLogger = new ();

    [Test]
    public void HandleUnsuccessfulHttpResponse_Throws_BadRequestException_For_Bad_Request()
    {
        var requestsClient = new StubbedRequestsClient(Mock.Of<IPolicyRegistry<string>>(), _mockLogger.Object);
        var badRequestMessage = new ValidationProblemDetails(new Dictionary<string, string[]> { { "Test Error", new[] { "ErrorMessage" } } })
        {
            Title = "Message title",
            Status = (int)HttpStatusCode.BadRequest
        };
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(JsonSerializer.Serialize(badRequestMessage)),
            RequestMessage = new HttpRequestMessage(HttpMethod.Get, "Request/Path")
        };

        //Act Assert
        var ex = Assert.ThrowsAsync<BadRequestException>(async () => await requestsClient.HandleUnsucessfulHttpResponse(httpResponseMessage)
            , "BadRequestException has not been thrown");

        Assert.That(badRequestMessage.Title.Equals(ex.Message), "Exception message does not match expected title");
        Assert.That(badRequestMessage.Status.Equals((int)ex.StatusCode), "Exception status code does not match expected status code");
        ex.Errors.Should().BeEquivalentTo(badRequestMessage.Errors);
    }

    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.Conflict)]
    public void HandleUnsuccessfulHttpResponse_Throws_BadRequestExceptions(HttpStatusCode statusCode)
    {
        var requestsClient = new StubbedRequestsClient(Mock.Of<IPolicyRegistry<string>>(), _mockLogger.Object);
        var httpResponseMessage = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(JsonSerializer.Serialize("Error was thrown")),
            RequestMessage = new HttpRequestMessage(HttpMethod.Get, "Request/Path")
        };

        //Act Assert
        var ex = Assert.ThrowsAsync<BadRequestException>(async () => await requestsClient.HandleUnsucessfulHttpResponse(httpResponseMessage)
            , "BadRequestException has not been thrown");
        ex.StatusCode.Should().Be(statusCode);
    }

    [Test]
    public async Task HandleUnsuccessfulHttpResponse_Doesnt_Throw_For_Success_Status()
    {
        var requestsClient = new StubbedRequestsClient(Mock.Of<IPolicyRegistry<string>>(), _mockLogger.Object);
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize("Successful response")),
            RequestMessage = new HttpRequestMessage(HttpMethod.Get, "Request/Path")
        };

        //Act
        await requestsClient.HandleUnsucessfulHttpResponse(httpResponseMessage);

        //Assert
        Assert.Pass();
    }
}
