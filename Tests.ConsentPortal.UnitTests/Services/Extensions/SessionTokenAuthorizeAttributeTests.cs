using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using PersonalProject.ConsentPortal.Models;
using PersonalProject.ConsentPortal.Services;
using PersonalProject.ConsentPortal.Services.Extensions;
using FluentAssertions;
using System.Text;

namespace Tests.ConsentPortal.UnitTests.Services.Extensions;

internal class SessionTokenAuthorizeAttributeTests
{
    private const string ValidToken = "Valid";
    private const string InvalidToken = "Invalid";
    private Mock<ISessionAuthorizationService> _mockSessionAuthorizationService = null!;
    private Mock<IHttpContextAccessor> _mockHttpContextAccessor = null!;

    private AuthorizationFilterContext GetTestFilterContext(HttpContext httpContext)
    {
        var actionContext = new ActionContext(httpContext,
                new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

        var filterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata> { });

        return filterContext;
    }

    private SessionTokenAuthorizeAttribute GenerateSystemUnderTest()
        => new SessionTokenAuthorizeAttribute(_mockSessionAuthorizationService.Object, _mockHttpContextAccessor.Object);

    [OneTimeSetUp]
    public void Setup()
    {
        _mockSessionAuthorizationService = new();
        _mockHttpContextAccessor = new();
        _mockSessionAuthorizationService.Setup(service => service.ValidateSessionToken(ValidToken)).Returns(true);
        _mockSessionAuthorizationService.Setup(service => service.ValidateSessionToken(InvalidToken)).Returns(false);
        _mockSessionAuthorizationService.Setup(service => service.ValidateSessionToken(string.Empty)).Returns(true);
    }

    private AuthorizationFilterContext SetupHttpContext(string token)
    {
        var mockSession = new Mock<ISession>();
        byte[] dummy = Encoding.UTF8.GetBytes(token);
        mockSession.Setup(x => x.TryGetValue(Constants.SessionAuthorizationTokenKey, out dummy!));
        _mockHttpContextAccessor.Setup(s => s.HttpContext!.Session).Returns(mockSession.Object);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(context => context.Response).Returns(new Mock<HttpResponse>().Object);
        return GetTestFilterContext(mockHttpContext.Object);
    }

    [TestCase(ValidToken)]
    public void OnAuthorization_Accepts_Valid_Token(string token)
    {
        //Arrange
        var filterContext = SetupHttpContext(token);
        var systemUnderTest = GenerateSystemUnderTest();

        //Act
        systemUnderTest.OnAuthorization(filterContext);

        //Assert
        filterContext.Result.Should().BeNull();
    }

    [TestCase(InvalidToken)]
    [TestCase("")]
    public void OnAuthorization_Rejects_Invalid_Token(string token)
    {
        //Arrange
        //Arrange
        var filterContext = SetupHttpContext(token);
        var systemUnderTest = GenerateSystemUnderTest();

        //Act
        systemUnderTest.OnAuthorization(filterContext);

        //Assert
        filterContext.Result.Should().BeOfType(typeof(RedirectToPageResult));
    }
}
