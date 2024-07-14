using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.ConsentPortal.UnitTests.TestHelpers;

internal static class LoggerExtensions
{
    internal static void VerifyNumberOfLoggerCalls<T>(this Mock<ILogger<T>> logger, int numberOfCalls, LogLevel logLevel)
        => logger.Verify(logger =>
            logger.Log(
                It.Is<LogLevel>(l => l == logLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Exactly(numberOfCalls));

    internal static void VerifyNumberOfLogErrorCalls<T>(this Mock<ILogger<T>> logger, int numberOfCalls)
        => logger.VerifyNumberOfLoggerCalls(numberOfCalls, LogLevel.Error);
}
