using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonalProject.ConsentPortal.Services.Extensions;

public class SessionTokenAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly ISessionAuthorizationService _sessionAuthorizationService;
    private readonly ISession _session;

    public SessionTokenAuthorizeAttribute(ISessionAuthorizationService sessionAuthorizationService,
        IHttpContextAccessor httpContextAccessor)
    {
        _sessionAuthorizationService = sessionAuthorizationService;
        _session = httpContextAccessor.HttpContext!.Session;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string? sessionValue = _session.GetString("SessionId");

        if (!string.IsNullOrEmpty(sessionValue)) 
        { 
            var tokenIsValid = _sessionAuthorizationService.ValidateSessionToken(sessionValue);

            if (tokenIsValid) return;
        }

        context.Result = new RedirectToPageResult("./SessionExpired");
        context.HttpContext.Response.StatusCode = 401;
    }
}
