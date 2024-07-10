using PersonalProject.Domain.Request;

namespace PersonalProject.ConsentPortal.Services;

public interface IConsentService
{
    Task<TokenVerificationResult> VerifyToken(string token);
    Task<ConsentDetails> GetConsentDetails(string appRef);
    Task<bool> RegisterCosent(string appRef);
}

public class ConsentService : IConsentService
{
    public Task<ConsentDetails> GetConsentDetails(string appRef)
    {
        throw new NotImplementedException();
    }

    public Task<TokenVerificationResult> VerifyToken(string token)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RegisterCosent(string appRef)
    {
        throw new NotImplementedException();
    }
}
