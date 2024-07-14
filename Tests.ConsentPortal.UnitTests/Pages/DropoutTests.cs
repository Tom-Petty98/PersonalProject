using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalProject.ConsentPortal.Models;
using PersonalProject.ConsentPortal.Pages.Consent;

namespace Tests.ConsentPortal.UnitTests.Pages;

[TestFixture]
public class DropoutTests
{
    private DropoutModel _systemUnderTest = null!;
    private Mock<ILogger<DropoutModel>> _mockLogger = new();

    [SetUp]
    public void TestCaseSetup()
    {
        _systemUnderTest = new(_mockLogger.Object);
    }

    [TestCase(DropoutEnum.SessionExpired, "Session expired")]
    [TestCase(DropoutEnum.AlreadyGiven, "Consent already given")]
    [TestCase(DropoutEnum.LinkExpired, "The email link has expired")]
    public void OnGet_Should_Display_Correct_PageTitle(DropoutEnum dropoutEnum, string title)
    {
        //Act
        _systemUnderTest.OnGet(dropoutEnum);

        //Assert
        _systemUnderTest.Heading.Should().Contain(title);
    }

    [TestCase(DropoutEnum.SessionExpired, "If you still need to give consent please click on the email link again.")]
    [TestCase(DropoutEnum.AlreadyGiven, "You have already give consent for this applicaiton.")]
    [TestCase(DropoutEnum.LinkExpired, "The email link has expired you will need to let your installer know so that consent can be reissued.")]
    public void OnGet_Should_Display_Correct_BodyContent(DropoutEnum dropoutEnum, string content)
    {
        //Act
        _systemUnderTest.OnGet(dropoutEnum);

        //Assert
        _systemUnderTest.BodyContent.Should().Contain(content);
    }
}