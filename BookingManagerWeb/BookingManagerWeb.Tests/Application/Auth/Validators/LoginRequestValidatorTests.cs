using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Application.Auth.Validators;

namespace BookingManagerWeb.Tests.Application.Auth.Validators;

public class LoginRequestValidatorTests
{
    private readonly LoginRequestDtoValidator _loginRequestDtoValidator = new();

    private static LoginRequestDto GetLoginRequestDto() =>
        new LoginRequestDto() { Email = "someemail@gmail.com", Password = "Qwerty123!" };
    
    [Fact] 
    public void ShouldHaveNoErrors_WhenValidLoginRequestDto()
    {
        var loginRequestDto = GetLoginRequestDto();
        var result = _loginRequestDtoValidator.Validate(loginRequestDto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldHaveErrors_WhenInvalidEmail()
    {
        var loginRequestDto = GetLoginRequestDto() with {Email = "invalidemail.com"};
        var result = _loginRequestDtoValidator.Validate(loginRequestDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ShouldHaveErrors_WhenEmailEmpty()
    {
        var loginRequestDto = GetLoginRequestDto() with {Email = ""};
        var result = _loginRequestDtoValidator.Validate(loginRequestDto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ShouldHaveErrors_WhenPasswordEmpty()
    {
        var loginRequestDto = GetLoginRequestDto() with {Password = ""};
        var result = _loginRequestDtoValidator.Validate(loginRequestDto);
        Assert.False(result.IsValid);
    }

}