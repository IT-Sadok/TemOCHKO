using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Domain.Constants;
using BookingManagerWeb.Infrastructure.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace BookingManagerWeb.Application.Auth;

// TODO use wrappers instead of direct implementations
public class AuthService(
    UserManager<ApplicationUser> userManager, 
    RoleManager<IdentityRole> roleManager) : IAuthService
{
    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto, CancellationToken ct) 
    {
        if (registerRequestDto.Role is not (Roles.Client or Roles.Host))
        {
            throw new AuthException("User must have a client or host role");
        }
        
        if (!await roleManager.RoleExistsAsync(registerRequestDto.Role))
        {
            throw new AuthException("Role does not exist");
        }
        
        var user = registerRequestDto.Adapt<ApplicationUser>();

        var result = await userManager.CreateAsync(user, registerRequestDto.Password);
        if (!result.Succeeded)
        {
            throw new  AuthException("User creation failed");
        }
        
        var roleSucceeded = await userManager.AddToRoleAsync(user, registerRequestDto.Role);
        if (!roleSucceeded.Succeeded)
        {
            throw new AuthException("Role creation failed");       
        }

        return (user, registerRequestDto.Role).Adapt<RegisterResponseDto>();
    }
}