using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PersonalProject.Domain.Request;
using Polly.Registry;
using System.Net;
using Tests.ConsentPortal.UnitTests.TestHelpers;

namespace Tests.ConsentPortal.UnitTests.Services.Extensions;

[TestFixture]
public class BaseClientRequestMethodTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    private readonly Mock<ILogger<StubbedRequestsClient>> _mockLogger = new();

    [OneTimeSetUp]
    public void TestFictureSetup()
    {
        Mock<ILogger<StubbedRequestsClient>> logger = new();
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("Test content") });
    }

    [Test]
    public async Task GetAsync_Without_Audit_Log_Params_Returns_String()
    {
        //Arrange
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        httpClient.BaseAddress = new Uri("http://www.mytest.com");

        var systemUnderTest = new StubbedRequestsClient(Mock.Of<IPolicyRegistry<string>>(), _mockLogger.Object);

        //Act
        var result = await systemUnderTest.GetAsync<string>(httpClient, "TestTarget");

        //Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task GetAsync_Returns_String()
    {
        //Arrange
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        httpClient.BaseAddress = new Uri("http://www.mytest.com");

        var systemUnderTest = new StubbedRequestsClient(Mock.Of<IPolicyRegistry<string>>(), _mockLogger.Object);
        var auditParams = new AuditLogParameters { Username = "Test", UserType = "test" };

        //Act
        var result = await systemUnderTest.GetAsync<string>(httpClient, "TestTarget", auditParams);

        //Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task PostAsync_With_HttpContentBody_Returns_String()
    {
        //Arrange
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        httpClient.BaseAddress = new Uri("http://www.mytest.com");

        var systemUnderTest = new StubbedRequestsClient(Mock.Of<IPolicyRegistry<string>>(), _mockLogger.Object);
        var auditParams = new AuditLogParameters { Username = "Test", UserType = "test" };
        var content = new Dictionary<string, string> { { "test", "testValue" } };

        //Act
        var result = await systemUnderTest.PostAsync<FormUrlEncodedContent, string>(httpClient, "TestTarget", new FormUrlEncodedContent(content), auditParams);

        //Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task PostAsync_Without_Body_Returns_String()
    {
        //Arrange
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        httpClient.BaseAddress = new Uri("http://www.mytest.com");

        var systemUnderTest = new StubbedRequestsClient(Mock.Of<IPolicyRegistry<string>>(), _mockLogger.Object);
        var auditParams = new AuditLogParameters { Username = "Test", UserType = "test" };

        //Act
        var result = await systemUnderTest.PostAsync<string>(httpClient, "TestTarget", auditParams);

        //Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task PutAsync_With_Body_Returns_String()
    {
        //Arrange
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        httpClient.BaseAddress = new Uri("http://www.mytest.com");

        var systemUnderTest = new StubbedRequestsClient(Mock.Of<IPolicyRegistry<string>>(), _mockLogger.Object);
        var auditParams = new AuditLogParameters { Username = "Test", UserType = "test" };

        //Act
        var result = await systemUnderTest.PutAsync<string, string>(httpClient, "TestTarget", "Test", auditParams);

        //Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task DeleteAsync_With_Body_Returns_Bool()
    {
        //Arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("true") });

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        httpClient.BaseAddress = new Uri("http://www.mytest.com");

        var systemUnderTest = new StubbedRequestsClient(Mock.Of<IPolicyRegistry<string>>(), _mockLogger.Object);
        var auditParams = new AuditLogParameters { Username = "Test", UserType = "test" };

        //Act
        var result = await systemUnderTest.DeleteAsync<string, bool>(httpClient, "TestTarget", "Test", auditParams);

        //Assert
        result.Should().BeTrue();
    }
}
