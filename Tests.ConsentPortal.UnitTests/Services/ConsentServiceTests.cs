using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PersonalProject.ConsentPortal.Services;
using PersonalProject.Domain.Request;
using Polly.Registry;
using System.Net.Http.Json;

namespace Tests.ConsentPortal.UnitTests.Services;

[TestFixture]
public class ConsentServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    private Mock<IHttpClientFactory> _mockHttpClientFactory = new();
    private Mock<ILogger<ConsentService>> _mockLogger = new();

    private ConsentService GenerateSystemUnderTest()
        => new(_mockHttpClientFactory.Object, _mockLogger.Object, Mock.Of<IPolicyRegistry<string>>());

    [Test]
    public async Task VerifyToken_Calls_Api_Returns_Valid_Response()
    {
        //Arrange
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var dto =  new TokenVerificationResult()
        {
            TokenAccepted = true,
            EntityRef = "App10101",
            ExpiryDate = DateTime.UtcNow.AddDays(6),
        };
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = JsonContent.Create(dto)
        };

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        var httpClient = new HttpClient(mockHandler.Object);
        httpClient.BaseAddress = new Uri("http://www.mytest.com");
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var systemUnderTest = GenerateSystemUnderTest();

        //Act
        var result = await systemUnderTest.VerifyToken("ValidToken");

        //Assert
        result.Should().BeEquivalentTo(dto);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get &&
                req.RequestUri == new Uri("http://www.mytest.com/Consent/VerifyToken/ValidToken")),
            ItExpr.IsAny<CancellationToken>());

    }
}
