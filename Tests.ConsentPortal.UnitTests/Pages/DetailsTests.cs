using FluentAssertions;
using PersonalProject.ConsentPortal.Models;
using PersonalProject.ConsentPortal.Pages.Consent;
using PersonalProject.ConsentPortal.Services.Extensions;

namespace Tests.ConsentPortal.UnitTests.Pages;

[TestFixture]
public class DetailsTests : PageModelTestBase
{
    private DetailsModel _systemUnderTest = null!;

    [SetUp]
    public override void TestCaseSetup()
    {
        _systemUnderTest = new();
        base.TestCaseSetup();
    }

    [Test]
    public void OnGet_Populates_Consent_Details()
    {
        //Arrange
        var httpContext = CreateEmptyHttpContext();

        _systemUnderTest.PageContext = CreatePageContext(httpContext);
        _systemUnderTest.PageContext.HttpContext.Session.Put(Constants.ConsentDetailsSessionKey, ConsentDetails);

        //Act
        _systemUnderTest.OnGet();

        //Assert
        _systemUnderTest.ConsentDetails.Should().BeEquivalentTo(ConsentDetails);
    }

    [Test]
    public void OnPost_Adds_Displays_Errors_Of_Not_Checked()
    {
        //Arrange
        var httpContext = CreateEmptyHttpContext();
        _systemUnderTest.PageContext = CreatePageContext(httpContext);

        //Act
        _systemUnderTest.OnPost();

        //Assert
        _systemUnderTest.ModelState.ErrorCount.Should().Be(2);
    }
}
