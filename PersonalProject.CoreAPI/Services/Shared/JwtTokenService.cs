using Microsoft.IdentityModel.Tokens;
using PersonalProject.Domain.Request;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalProject.CoreAPI.Services.Shared;

public interface IJwtTokenService
{
    string GenerateToken(string entityRef, DateTime expiryDate);
    TokenVerificationResult VerifyToken(string token);
}

public class JwtTokenService : IJwtTokenService
{
    private readonly Config _config;

    public JwtTokenService(Config config)
    {
        _config = config;
    }

    public string GenerateToken(string entityRefNum, DateTime expiryDate)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.EmailTokenSecret));
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("EntityRef", entityRefNum), 
                new Claim("ExpiryDate", expiryDate.ToString()),
            }),
            Expires = expiryDate,
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }

    public TokenVerificationResult VerifyToken(string token)
    {
        if (token == null) throw new ArgumentNullException(nameof(token));

        var validationResult = new TokenVerificationResult() { TokenAccepted = false };

        if (!ValidateToken(token)) return validationResult;

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (securityToken == null) return validationResult;

        validationResult.TokenAccepted = true;
        validationResult.EntityRef = securityToken.Claims.First(claim => claim.Type == "EntityRef").Value;
        validationResult.ExpiryDate = DateTime.Parse(securityToken.Claims.First(claim => claim.Type == "ExpiryDate").Value);

        return validationResult;
    }

    private bool ValidateToken(string token)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.EmailTokenSecret));
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }
}
