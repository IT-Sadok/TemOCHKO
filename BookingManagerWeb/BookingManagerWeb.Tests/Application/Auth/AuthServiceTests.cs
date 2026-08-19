using BookingManagerWeb.Application.Auth;
using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Application.Auth.Mapping;
using BookingManagerWeb.Application.Auth.Services;
using BookingManagerWeb.Domain.Constants;
using BookingManagerWeb.Infrastructure.Auth;
using BookingManagerWeb.Infrastructure.Identity;
using BookingManagerWeb.Infrastructure.Identity.Wrappers;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shouldly;

namespace BookingManagerWeb.Tests.Application.Auth;

public class AuthServiceTests
{
    private readonly IAuthService _systemUnderTest;
    private readonly Mock<IUserManagerWrapper> _userManagerMock = new();
    private readonly Mock<IRoleManagerWrapper> _roleManagerMock = new();
    private readonly Mock<IJwtService> _jwtServiceMock = new();

    public AuthServiceTests()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        new RegisterAuthMapping().Register(config);

        var mapper = new Mapper(config);

        _systemUnderTest = new AuthService(
            _userManagerMock.Object,
            _roleManagerMock.Object,
            mapper,
            _jwtServiceMock.Object
        );

    }

    [Fact]
    public async Task RegisterAsync_WhenRoleIsInvalid_ThrowsAuthException()
    {
        var request = new RegisterRequestDto { Role = "SomeRole" };
        var exception = await Should.ThrowAsync<AuthException>(() =>
            _systemUnderTest.RegisterAsync(request, CancellationToken.None));

        exception.ShouldNotBeNull();
        exception.Message.ShouldBe("User must have a client or host role");
    }

    [Fact]
    public async Task RegisterAsync_WhenRoleDoesNotExist_ThrowsAuthException()
    {
        var request = new RegisterRequestDto() { Role = "Client" };

        _roleManagerMock.Setup(x => x.RoleExistsAsync(request.Role, CancellationToken.None)).ReturnsAsync(false);

        var exception = await Should.ThrowAsync<AuthException>(() =>
            _systemUnderTest.RegisterAsync(request, CancellationToken.None));

        exception.ShouldNotBeNull();
        exception.Message.ShouldBe("Role does not exist");
    }

    [Fact]
    public async Task RegisterAsync_WhenUserCreationFailed_ThrowsAuthException()
    {
        var request = new RegisterRequestDto() {Role = "Host", Password = "ValidPass123!"};
        
        _roleManagerMock.Setup(x => x.RoleExistsAsync(request.Role, CancellationToken.None)).ReturnsAsync(true);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), 
            request.Password, It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Failed());
        
        var exception = await Should.ThrowAsync<AuthException>(() =>
            _systemUnderTest.RegisterAsync(request, CancellationToken.None));
        
        exception.ShouldNotBeNull();
        exception.Message.ShouldBe("User creation failed");
    }

    [Fact]
    public async Task RegisterAsync_WhenRoleAssignmentFailed_ThrowsAuthException()
    {
        var request = new RegisterRequestDto() {Role = "Client", Password = "ValidPass123!"};
        
        _roleManagerMock.Setup(x => x.RoleExistsAsync(request.Role, CancellationToken.None)).ReturnsAsync(true);
        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), request.Role, It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Failed());
        
        var exception = await Should.ThrowAsync<AuthException>(() => _systemUnderTest.RegisterAsync(request, CancellationToken.None));
        
        exception.ShouldNotBeNull();
        exception.Message.ShouldBe("Role assignment failed");
    }

    [Fact]
    public async Task RegisterAsync_WhenSuccess_ReturnsRegisterResponseDto()
    {
        var request = new RegisterRequestDto() {Role = "Client", Password = "ValidPass123!"};
        
        _roleManagerMock.Setup(x =>  x.RoleExistsAsync(request.Role, CancellationToken.None)).ReturnsAsync(true);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password, It.IsAny<CancellationToken>() )).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), request.Role, It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Success);
        
        var jwtToken = "some-jwt-token";
        _jwtServiceMock.Setup(x => x.GenerateToken(It.IsAny<ApplicationUser>())).Returns(jwtToken);
        
        var result = await _systemUnderTest.RegisterAsync(request, CancellationToken.None);
        
        result.ShouldNotBeNull();
        result.AccessToken.ShouldBe(jwtToken);
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ThrowsAuthException()
    {
        var request = new LoginRequestDto { Email = "unknown@test.com", Password = "Password123!" };
        
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApplicationUser)null!); 

        var exception = await Should.ThrowAsync<AuthException>(() => 
            _systemUnderTest.LoginAsync(request, CancellationToken.None));

        exception.Message.ShouldBe("User not found");
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsInvalid_ThrowsAuthException()
    {
        var request = new LoginRequestDto { Email = "test@test.com", Password = "WrongPassword!" };
        var user = new ApplicationUser { Email = request.Email };
        
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, request.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false); 

        var exception = await Should.ThrowAsync<AuthException>(() => 
            _systemUnderTest.LoginAsync(request, CancellationToken.None));
        
        exception.Message.ShouldBe("Password is invalid");
    }

    [Fact]
    public async Task LoginAsync_ReturnsLoginResponseDtoWithToken_WhenSuccess()
    {
        var request = new LoginRequestDto { Email = "someemail@test.com", Password = "Qwerty123!" };
        var user = new ApplicationUser { Email = request.Email };
        var expectedToken = "test-jwt-token";
        
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, request.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _jwtServiceMock.Setup(x => x.GenerateToken(user))
            .Returns(expectedToken);

        var result = await _systemUnderTest.LoginAsync(request, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Token.ShouldBe(expectedToken);
    }
}