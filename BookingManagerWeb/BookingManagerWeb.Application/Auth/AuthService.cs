using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Domain.Constants;
using BookingManagerWeb.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace BookingManagerWeb.Application.Auth;

// TODO use wrappers instead of direct implementations
public class AuthService(
    UserManager<ApplicationUser> userManager, 
    RoleManager<IdentityRole> roleManager) : IAuthService
{
    public Task<RegisterResponseDto> Register(RegisterRequestDto registerRequestDto, CancellationToken ct) 
    {
        // TODO check if role is appropriate for user to authorize
        // TODO roleManager.RoleExists
        // TODO map user 
        // TODO userManager.CreateAsync
        // TODO userManager.AddToRoleAsync
        
        throw new NotImplementedException();
    }

    public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}