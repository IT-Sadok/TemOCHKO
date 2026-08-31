using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Application.Auth.Validators;
using BookingManagerWeb.Domain.Constants;

namespace BookingManagerWeb.Tests.Application.Auth.Validators;

public class RegisterRequestValidatorTests
{
    private readonly RegisterRequestDtoValidator _registerRequestDtoValidator = new();
    
    private static RegisterRequestDto CreateRegisterRequestDto() =>
        new RegisterRequestDto { Email = "someemail@gmail.com", FirstName = "Test", LastName = "Test", Password = "Qwerty123!", Role = Roles.Client};
    
    [Fact]
    public void ShouldHaveNoErrors_WhenRegisterRequestIsValid()
    {
        var registerRequestDto = CreateRegisterRequestDto();
        var result = _registerRequestDtoValidator.Validate(registerRequestDto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldHaveErrors_WhenEmailIsInvalid()
    {
        var registerRequestDto = CreateRegisterRequestDto() with {Email = "invalidemail.com"};
        var result = _registerRequestDtoValidator.Validate(registerRequestDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ShouldHaveErrors_WhenPasswordIsPlainText()
    {
        var registerRequestDto = CreateRegisterRequestDto() with  {Password = "invalidepassword"};
        var result = _registerRequestDtoValidator.Validate(registerRequestDto);
        Assert.False(result.IsValid);
    }
    

    [Fact]
    public void ShouldHaveErrors_WhenPasswordDoesNotIncludeUppercaseCharacters()
    {
        var registerRequestDto = CreateRegisterRequestDto() with {Password = "invalidpass123!"};
        var result = _registerRequestDtoValidator.Validate(registerRequestDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ShouldHaveErrors_WhenPasswordIsTooShort()
    {
        var registerRequestDto = CreateRegisterRequestDto() with {Password = "Hi123"};
        var result = _registerRequestDtoValidator.Validate(registerRequestDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ShouldHaveErrors_WhenPasswordIsNull()
    {
        var registerRequestDto = CreateRegisterRequestDto() with {Password = null};
        var result = _registerRequestDtoValidator.Validate(registerRequestDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ShouldHaveErrors_WhenRoleIsUndefined()
    {
        var registerRequestDto = CreateRegisterRequestDto() with {Role = "RandomRole"};
        var result = _registerRequestDtoValidator.Validate(registerRequestDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ShouldHaveErrors_WhenFirstNameIsEmpty()
    {
        var registerRequestDto = CreateRegisterRequestDto() with {FirstName = string.Empty};
        var result = _registerRequestDtoValidator.Validate(registerRequestDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ShouldHaveErrors_WhenLastNameIsEmpty()
    {
        var registerRequestDto = CreateRegisterRequestDto() with { LastName = string.Empty };
        var result = _registerRequestDtoValidator.Validate(registerRequestDto);
        Assert.False(result.IsValid);
    }
}