using BookingManagerWeb.Infrastructure.Auth;
using BookingManagerWeb.Infrastructure.Auth.Options;
using BookingManagerWeb.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Shouldly;

namespace BookingManagerWeb.Tests.Infrastructure.Auth;

public class JwtServiceTests
{
    private readonly JwtOptions _jwtOptions = new()
    {
        SecretKey = "this-is-a-super-secret-dummy-key-for-testing-purposes-only!", 
        Audience = "test-audience",
        Issuer = "test-issuer",
        ExpiresInMinutes = 30
    };
    
    private readonly IJwtService _jwtService; 
    
    public JwtServiceTests()
    {
        var options = Options.Create(_jwtOptions);
        
        _jwtService = new JwtService(options);
    }

    [Fact]
    public void GenerateToken_ShouldContainValidUserId_WhenCalled()
    {
        var userId =  Guid.NewGuid().ToString();
        var user = new ApplicationUser
        {
            Id = userId,
        };
        
        var token = _jwtService.GenerateToken(user);

        var handler = new JsonWebTokenHandler();
        var parsedToken = handler.ReadJsonWebToken(token);

        parsedToken.Subject.ShouldBe(userId);
    }

    [Fact]
    public void GenerateToken_ShouldContainValidIssuerAndAudience_WhenCalled()
    {
        var userId =  Guid.NewGuid().ToString();
        var user = new ApplicationUser
        {
            Id = userId,
        };
        
        var token = _jwtService.GenerateToken(user);

        var handler = new JsonWebTokenHandler();
        var parsedToken = handler.ReadJsonWebToken(token);

        parsedToken.Issuer.ShouldBe(_jwtOptions.Issuer);
        parsedToken.Audiences.ShouldContain(_jwtOptions.Audience);
    }

    [Fact]
    public void GenerateToken_ShouldBeCorrectExpirationTime_WhenCalled()
    {
        var userId =  Guid.NewGuid().ToString();
        var user = new ApplicationUser
        {
            Id = userId,
        };
        
        var expirationOptions = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes);
        
        var token = _jwtService.GenerateToken(user);
        
        var handler = new JsonWebTokenHandler();
        var parsedToken = handler.ReadJsonWebToken(token);

        var expirationInToken = parsedToken.ValidTo;
        expirationOptions.ShouldBe(expirationInToken, TimeSpan.FromSeconds(2));
    }
    
    [Fact]
    public void GenerateToken_ShouldGenerateCorrectToken_WhenCalled()
    {
        var userId =  Guid.NewGuid().ToString();
        var user = new ApplicationUser
        {
            Id = userId,
        };
        
        var token = _jwtService.GenerateToken(user);
        
        var handler = new JsonWebTokenHandler();
        var parsedToken = handler.ReadJsonWebToken(token);

        parsedToken.Subject.ShouldBe(userId);
        parsedToken.Issuer.ShouldBe(_jwtOptions.Issuer);
        parsedToken.Audiences.ShouldContain(_jwtOptions.Audience);
        parsedToken.ValidTo.ShouldBe(DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes),  TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void GenerateToken_ThrowsException_WhenProvidedWithShortSecret()
    {
        JwtOptions newOptions = new()
        {
            SecretKey = "this-is-a-short-key", 
            Audience = "test-audience",
            Issuer = "test-issuer",
            ExpiresInMinutes = 30
        };

        var service = new JwtService(Options.Create(newOptions));
        var user = new ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
        };
        
        var exception = Should.Throw<Exception>(() => service.GenerateToken(user));
        exception.ShouldNotBeNull();
    }
    
}