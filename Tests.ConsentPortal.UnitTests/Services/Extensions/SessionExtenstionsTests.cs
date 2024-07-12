using FluentAssertions;
using Microsoft.AspNetCore.Http;
using PersonalProject.ConsentPortal.Services.Extensions;
using PersonalProject.Domain.Request;
using System.Text.Json;
using Tests.ConsentPortal.UnitTests.TestHelpers;

namespace Tests.ConsentPortal.UnitTests.Services.Extensions;

public class SessionExtenstionsTests
{
    private FakeHttpSession _session = new FakeHttpSession();

    [SetUp]
    public void TestCaseSetup()
    {
        _session = new FakeHttpSession();
    }

    [Test]
    public void Put_Adds_Simple_Value_To_Session()
    {
        //Arrange
        var key = "MyKey";
        var value = "Hello, world";
        var seralizedValue = JsonSerializer.Serialize(value);

        //Act
        _session.Put(key, value);

        //Assert
        var rawValue = _session.GetString(key);
        rawValue.Should().NotBeNull().And.Be(seralizedValue);
    }

    [Test]
    public void Put_Adds_Complex_Value_To_Session()
    {
        //Arrange
        var key = "MyKey";
        var value = new ConsentDetails { AppRefNumber = "GID11111111" };
        var seralizedValue = JsonSerializer.Serialize(value);

        //Act
        _session.Put(key, value);

        //Assert
        var rawValue = _session.GetString(key);
        rawValue.Should().NotBeNull().And.Be(seralizedValue);
    }

    [Test]
    public void GetOrDefault_Returns_Null_For_Non_Existing_Key()
    {
        //Arrange
        var key = "MyKey";

        //Act
        var retrievedValue = _session.GetOrDefault<string>(key);

        //Assert
        retrievedValue.Should().BeNull();
    }

    [Test]
    public void GetOrDefault_Returns_Complex_Value()
    {
        //Arrange
        var key = "MyKey";
        var value = new ConsentDetails { AppRefNumber = "GID11111111" };
        _session.Put(key, value);

        //Act
        var retrievedValue = _session.GetOrDefault<ConsentDetails>(key);

        //Assert
        retrievedValue.Should().NotBeNull().And.BeEquivalentTo(value);
    }
}
