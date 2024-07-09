using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalProject.ConsentPortal.Services;

public interface ISessionAuthorizationService
{
    string ExtendSessionToken(string token);
    string GenerateSessionToken(string appId);
    bool ValidateSessionToken(string token);
}

public class SessionAuthorizationService : ISessionAuthorizationService
{
    private readonly string _consentTokenSecret;

    const int SessionTokenExpiryMinutes = 30;
    const string AppIdClaimName = "EntityId";
    const string ExpiryDateClaimName = "ExpiryDate";

    public SessionAuthorizationService(string consentTokenSecret)
    {
        _consentTokenSecret = consentTokenSecret;
    }

    public string ExtendSessionToken(string token)
    {
        var sessionToken = GetSessionToken(token);

        if (sessionToken is JwtSecurityToken jwtSessionToken)
        {
            var appIdClaim = jwtSessionToken.Claims.FirstOrDefault(claim => claim.Type == AppIdClaimName);

            if (appIdClaim != null) 
            { 
                return GenerateSessionToken(appIdClaim.Value);
            }
        }

        return string.Empty;
    }  

    public string GenerateSessionToken(string appId)
    {
        var tokenExpiryDateTime = DateTime.UtcNow.AddMinutes(SessionTokenExpiryMinutes);

        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_consentTokenSecret));
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(AppIdClaimName, appId.ToString()),
                new Claim(ExpiryDateClaimName, tokenExpiryDateTime.ToString()),
            }),
            Expires = tokenExpiryDateTime,
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }

    public bool ValidateSessionToken(string token)
    {
        var sessionToken = GetSessionToken(token);

        if (sessionToken == null)
            return false;

        var isTokenValid = sessionToken.ValidTo.CompareTo(DateTime.UtcNow) >= 0;
        return isTokenValid;
    }

    private SecurityToken? GetSessionToken(string token)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_consentTokenSecret));
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateAudience = false,
                ValidateIssuer = false
            }, out SecurityToken validatedToken);

            return validatedToken;
        }
        catch
        {
            return null;
        }
    }
}
