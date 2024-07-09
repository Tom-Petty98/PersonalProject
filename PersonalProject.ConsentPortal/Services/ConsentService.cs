using PersonalProject.Domain.Request;

namespace PersonalProject.ConsentPortal.Services;

public interface IConsentService
{
    Task<TokenVerificationResult> VerifyToken(string token);
    //Task<ConsentDetails> GetConsentDetails(int appId);
    Task<bool> RegisterCosent(int appId);
}

public class ConsentService : IConsentService
{
    public Task<bool> RegisterCosent(int appId)
    {
        throw new NotImplementedException();
    }

    public Task<TokenVerificationResult> VerifyToken(string token)
    {
        throw new NotImplementedException();
    }
}
