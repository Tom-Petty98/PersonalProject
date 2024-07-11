using PersonalProject.ConsentPortal.Services.Extensions;
using PersonalProject.Domain.Request;
using Polly.Registry;
using System.Security.Cryptography;

namespace PersonalProject.ConsentPortal.Services;

public interface IConsentService
{
    Task<TokenVerificationResult> VerifyToken(string token);
    Task<ConsentDetails> GetConsentDetails(string appRef);
    Task<bool> RegisterConsent(string appRef);
}

public class ConsentService : BaseRequestsClient<ConsentService>, IConsentService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string _clientName = "CoreAPI";
    public ConsentService(IHttpClientFactory httpClientFactory, ILogger<ConsentService> logger, IPolicyRegistry<string> polySettings)
        : base(polySettings, logger)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient BuildClient() => _httpClientFactory.CreateClient(_clientName);

    public async Task<ConsentDetails> GetConsentDetails(string appRef)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetConsentDetails));
        var target = $"Consent/GetConsentDetails/{appRef}";

        var responseObject = await GetAsync<ConsentDetails>(httpClient, target, null, null);
        return responseObject ?? new ConsentDetails();
    }

    public async Task<TokenVerificationResult> VerifyToken(string token)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(VerifyToken));
        var target = $"Consent/VerifyToken/{token}";

        var responseObject = await GetAsync<TokenVerificationResult>(httpClient, target, null, null);
        return responseObject ?? new TokenVerificationResult();
    }

    public async Task<bool> RegisterConsent(string appRef)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(RegisterConsent));
        var target = $"Consent/RegisterConsent/{appRef}";

        return await PostAsync<bool>(httpClient, target, null, null);
    }
}
