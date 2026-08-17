using System.Security.Claims;
using System.Text;
using BookingManagerWeb.Infrastructure.Auth.Options;
using BookingManagerWeb.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace BookingManagerWeb.Infrastructure.Auth;

public class JwtService(IOptions<JwtOptions> options) : IJwtService
{
    private JwtOptions JwtOptions { get; set; } = options.Value;

    public string GenerateToken(ApplicationUser user)
    {
        var secretKey = JwtOptions.SecretKey;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        
        var credentials  = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            ]), 
            Expires = DateTime.UtcNow.AddMinutes(JwtOptions.ExpiresInMinutes),
            SigningCredentials = credentials, 
            Issuer = JwtOptions.Issuer,
            Audience = JwtOptions.Audience,
        };

        var tokenHandler = new JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
            
        return token;
    }
}
