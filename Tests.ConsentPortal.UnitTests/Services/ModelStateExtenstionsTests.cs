using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PersonalProject.ConsentPortal.Services.Extensions;

namespace Tests.ConsentPortal.UnitTests.Services;

[TestFixture]
public class ModelStateExtenstionsTests
{
    [Test]
    public void HasError_Returns_True_If_Error_Exists()
    {
        //Arrange
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("TestError", "Something broke");

        //Act
        var isValid = modelState.HasError("TestError");

        //Assert
        isValid.Should().BeTrue();
    }

    [Test]
    public void HasError_Returns_False_If_Error_Doesnt_Exists()
    {
        //Arrange
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("TestError1", "Something broke");
        modelState.AddModelError("TestError2", "Something broke");
        modelState.AddModelError("TestError3", "Something broke");


        //Act
        var isValid = modelState.HasError("TestError4");

        //Assert
        isValid.Should().BeFalse();
    }
}
