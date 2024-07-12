using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalProject.ConsentPortal.Services.Extensions;
using PersonalProject.Domain.Request;
using Polly;
using Polly.Registry;

namespace Tests.ConsentPortal.UnitTests.Services.Extensions;

[TestFixture]
public class PollyExtensionsTests
{
    [Test]
    public void TryGetLogger_Gets_Logger()
    {
        //Arrange
        var mockLogger = Mock.Of<ILogger<PollyExtensionsTests>>();
        var context = new Context
        {
            { PollyContextKeys.Logger, mockLogger },
        };

        //Act Assert
        context.TryGetLogger(out var logger).Should().BeTrue();
        logger.Should().BeEquivalentTo(mockLogger);
    }

    [Test]
    public void TryGetLogger_No_Logger_Found()
    {
        //Arrange
        var context = new Context();

        //Act Assert
        context.TryGetLogger(out var logger).Should().BeFalse();
        logger.Should().BeNull();
    }

    [Test]
    public void AddPollyPolicies_New_Instance()
    {
        //Arrange
        IServiceCollection serviceCollection = new ServiceCollection();
        var config = new Dictionary<string, string>
        {
            { "Http500RetryCount", "3" },
            { "Http500RetryInterval", "500" }
        };

        var configuration = new ConfigurationBuilder().AddInMemoryCollection(config!).Build();
        var policyRegistry = new Dictionary<string, IAsyncPolicy<HttpResponseMessage>>()
        {
            { PollyContextKeys.RetryHttp500, PollyExtensions.Http500RetryPolicy(configuration) }
        };

        //Act
        serviceCollection.AddPollyPolicies(policyRegistry);

        //Assert
        var builder = serviceCollection.BuildServiceProvider();
        builder.GetService<IPolicyRegistry<string>>().Should().BeEquivalentTo(policyRegistry);
    }

    public void AddPollyPolicies_Existing_Instance_Adds_New_Key()
    {
        //Arrange
        IServiceCollection serviceCollection = new ServiceCollection();
        var config = new Dictionary<string, string>
        {
            { "Http500RetryCount", "3" },
            { "Http500RetryInterval", "500" }
        };

        var configuration = new ConfigurationBuilder().AddInMemoryCollection(config!).Build();
        var policyRegistry = new PolicyRegistry()
        {
            { PollyContextKeys.RetryHttp500, PollyExtensions.Http500RetryPolicy(configuration) }
        };

        var secoundPolicyRegistry = new Dictionary<string, IAsyncPolicy<HttpResponseMessage>>()
        {
            { "404", PollyExtensions.Http500RetryPolicy(configuration) }
        };
        serviceCollection.AddPolicyRegistry(policyRegistry);

        //Act
        serviceCollection.AddPollyPolicies(secoundPolicyRegistry);

        //Assert
        var builder = serviceCollection.BuildServiceProvider();
        builder.GetService<IPolicyRegistry<string>>().Should().HaveCount(2);
    }

    [Test]
    [TestCase(null, PollyContextKeys.RetryHttp500)]
    [TestCase(PollyContextKeys.RetryHttp500, PollyContextKeys.RetryHttp500)]
    [TestCase("404_Error", "404_Error")]
    public void BuildPollyParams(string? key, string? expectedKey)
    {
        //Arrange
        var expectedResult = new PollyParameters
        {
            PolicyKey = expectedKey,
            Source = PollyContextKeys.Source
        };

        //Act Assert
        PollyExtensions.BuildPollyParams(PollyContextKeys.Source, key).Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void BuildPollyContext()
    {
        //Arrange
        var logger = Mock.Of<ILogger<PollyExtensionsTests>>();
        var pollyParams = new PollyParameters
        {
            PolicyKey = "key",
            Source = "source",
            ContextParameters = new Dictionary<string, object>
            {
                { "NewKey", "NewValue" }
            }
        };

        var expectedContext = new Context
        {
            { PollyContextKeys.RetryCount, 0 },
            { PollyContextKeys.Source, "source" },
            { PollyContextKeys.Logger, logger },
            { "NewKey", "NewValue" }
        };

        //Act
        var context = pollyParams.BuildPollyContext(logger);

        //Assert
        context.Should().BeEquivalentTo(expectedContext);
    }
}
