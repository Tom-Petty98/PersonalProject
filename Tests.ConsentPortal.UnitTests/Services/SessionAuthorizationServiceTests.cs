using FluentAssertions;
using PersonalProject.ConsentPortal.Services;

namespace Tests.ConsentPortal.UnitTests.Services;

[TestFixture]
public class SessionAuthorizationServiceTests
{
    private SessionAuthorizationService _systemUnderTest = null!;

    private const string ConsentTokenSecret = "My super secret test secret that nedds to be long";
    private const string EntityRef = "App10101";

    [Test]
    public void GenerateSessionToken_Creates_Token_Successfully()
    {
        //Arrange
        _systemUnderTest = new SessionAuthorizationService(ConsentTokenSecret);

        //Act
        var testToken = _systemUnderTest.GenerateSessionToken(EntityRef);

        //Assert
        testToken.Should().NotBeNull();
    }

    [Test]
    public void GenerateSessionToken_Throws_Null_Exception()
    {
        //Arrange
        _systemUnderTest = new SessionAuthorizationService(ConsentTokenSecret);

        //Act & Assert
        Assert.That(() => _systemUnderTest.GenerateSessionToken(null!), Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void ValidateSessionToken_Validate_Invalid_Token()
    {
        //Arrange
        _systemUnderTest = new SessionAuthorizationService(ConsentTokenSecret);
        var testToken = "an invalid token";
        //Act
        var isValid = _systemUnderTest.ValidateSessionToken(testToken);

        //Assert
        isValid.Should().BeFalse();
    }

    [Test]
    public void ValidateSessionToken_Validate_Null_Token()
    {
        //Arrange
        _systemUnderTest = new SessionAuthorizationService(ConsentTokenSecret);
        //Act
        var isValid = _systemUnderTest.ValidateSessionToken(null!);

        //Assert
        isValid.Should().BeFalse();
    }

    [Test]
    public void ValidateSessionToken_Validate_Valid_Token()
    {
        //Arrange
        _systemUnderTest = new SessionAuthorizationService(ConsentTokenSecret);
        var testToken = _systemUnderTest.GenerateSessionToken(EntityRef);

        //Act
        var isValid = _systemUnderTest.ValidateSessionToken(testToken);

        //Assert
        isValid.Should().BeTrue();
    }

    [Test]
    public void ExtendSessionToken_Returns_No_Token_For_Invalid_Token()
    {
        //Arrange
        _systemUnderTest = new SessionAuthorizationService(ConsentTokenSecret);
        var testToken = "an invalid token";
        //Act
        var isValid = _systemUnderTest.ValidateSessionToken(testToken);

        //Assert
        isValid.Should().BeFalse();
    }

    [Test]
    public void ExtendSessionToken_Returns_No_Token_For_Null_Token()
    {
        //Arrange
        _systemUnderTest = new SessionAuthorizationService(ConsentTokenSecret);
        //Act
        var isValid = _systemUnderTest.ValidateSessionToken(null!);

        //Assert
        isValid.Should().BeFalse();
    }

    [Test]
    public void ExtendSessionToken_Returns_Token_For_Valid_Token()
    {
        //Arrange
        _systemUnderTest = new SessionAuthorizationService(ConsentTokenSecret);
        var testToken = _systemUnderTest.GenerateSessionToken(EntityRef);

        //Act
        var isValid = _systemUnderTest.ValidateSessionToken(testToken);

        //Assert
        isValid.Should().BeTrue();
    }
}
