using Castle.Core.Logging;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PersonalProject.ConsentPortal.Services.Extensions;
using PersonalProject.Domain.Request;
using Polly.Registry;
using Tests.ConsentPortal.UnitTests.TestHelpers;

namespace Tests.ConsentPortal.UnitTests.Services.Extensions;

[TestFixture]
public class PollyTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    private IConfiguration _configuration = null!;
    private readonly PolicyRegistry _pollyPolicyRegistry = new();

    [OneTimeSetUp]
    public void TestFixtureSetup()
    {
        var config = new Dictionary<string, string>
        {
            { "PollySettings:Http500RetryCount", "3" },
            { "PollySettings:Http500RetryInterval", "500" }
        };

        _configuration = new ConfigurationBuilder().AddInMemoryCollection(config!).Build();
        _pollyPolicyRegistry.Add(PollyContextKeys.RetryHttp500, PollyExtensions.Http500RetryPolicy(_configuration.GetSection("PollySettings")));
    }

    private PollyParameters PollyParameters => new()
    {
        PolicyKey = "RetryHttp500",
        Source = "MethodName"
    };

    private HttpClient BuildTestClient()
    {
        var client = new HttpClient(_mockHttpMessageHandler.Object);
        client.BaseAddress = new Uri("http://www.mytest.com");

        return client;
    }

    [Test]
    public void GetAsync_Returns_500_Error_3_Times_Return_HandleUnsuccessfulHttpResponse()
    {
        //Arrange
        Mock<ILogger<StubbedRequestsClient>> logger = new();
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError, Content = new StringContent("500 Error") });

        //Act
        var systemUnderTest = new StubbedRequestsClient(_pollyPolicyRegistry, logger.Object);
        var result = () => systemUnderTest.GetAsync<string>(BuildTestClient(), "TestTarget", null!, PollyParameters);

        //Assert
        result.Should().ThrowAsync<BadRequestException>();
    }

    [Test]
    public async Task ExecuteClientTaskWithPolicyAsync_Returns_500_Error_3_Times()
    {
        //Arrange
        Mock<ILogger<StubbedRequestsClient>> logger = new();
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError, Content = new StringContent("500 Error") });

        var httpClient = BuildTestClient();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "TestTarget");
        //Act
        var systemUnderTest = new StubbedRequestsClient(_pollyPolicyRegistry, logger.Object);

        //Assert
        using (new AssertionScope())
        {
            await systemUnderTest.ExecuteClientTaskWithPolicyAsync(PollyParameters, httpClient.SendAsync(request));

            logger.VerifyNumberOfLogErrorCalls(3);
        }
    }
}
