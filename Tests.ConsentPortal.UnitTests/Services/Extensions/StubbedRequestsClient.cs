using Microsoft.Extensions.Logging;
using PersonalProject.ConsentPortal.Services.Extensions;
using Polly.Registry;


namespace Tests.ConsentPortal.UnitTests.Services.Extensions;

public class StubbedRequestsClient : BaseRequestsClient<StubbedRequestsClient>
{
    public StubbedRequestsClient(IPolicyRegistry<string> policyRegistry,
        ILogger<StubbedRequestsClient> logger)
        : base(policyRegistry, logger)
    {
    }
}
