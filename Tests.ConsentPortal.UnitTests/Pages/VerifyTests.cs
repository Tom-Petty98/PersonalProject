using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalProject.ConsentPortal.Models;
using PersonalProject.ConsentPortal.Pages.Consent;
using PersonalProject.ConsentPortal.Services;
using PersonalProject.ConsentPortal.Services.Extensions;
using PersonalProject.Domain.Request;

namespace Tests.ConsentPortal.UnitTests.Pages;

[TestFixture]
internal class VerifyTests : PageModelTestBase
{
    private Mock<IConsentService> _consentServiceMock = new();
    private Mock<ISessionAuthorizationService> _mockSessionAuthorizationService = new();

    private TokenVerificationResult _validTokenResult = new TokenVerificationResult()
    {
        TokenAccepted = true,
        EntityRef = "App10101",
        ExpiryDate = DateTime.UtcNow.AddDays(6),
    };

    private VerifyModel GenerateSystemUnderTest()
        => new VerifyModel(_consentServiceMock.Object, _mockSessionAuthorizationService.Object);

    [SetUp]
    public override void TestCaseSetup()
    {
        base.TestCaseSetup();
    }

    [Test]
    public async Task OnGet_Populates_Session()
    {
        //Arrange
        var httpContext = CreateEmptyHttpContext();
        _mockSessionAuthorizationService.Setup(x => x.GenerateSessionToken(It.IsAny<string>())).Returns("ValidSessionToken");
        _consentServiceMock.Setup(x => x.VerifyToken(It.IsAny<string>())).ReturnsAsync(_validTokenResult);
        _consentServiceMock.Setup(x => x.GetConsentDetails(It.IsAny<string>())).ReturnsAsync(ConsentDetails);
        var systemUnderTest = GenerateSystemUnderTest();
        systemUnderTest.PageContext = CreatePageContext(httpContext);

        //Act
        var result = await systemUnderTest.OnGetAsync("ValidToken") as RedirectToPageResult;

        //Assert
        result!.PageName.Should().Be("./Details");

        var consentDetails = systemUnderTest.PageContext.HttpContext.Session.GetOrDefault<ConsentDetails>(Constants.ConsentDetailsSessionKey);
        consentDetails.Should().BeEquivalentTo(ConsentDetails);
    }
}
