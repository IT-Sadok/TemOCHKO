using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Domain.Constants;
using BookingManagerWeb.Infrastructure.Auth;
using BookingManagerWeb.Infrastructure.Identity;
using BookingManagerWeb.Infrastructure.Identity.Wrappers;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace BookingManagerWeb.Application.Auth.Services;

public class AuthService(
    IUserManagerWrapper userManager, 
    IRoleManagerWrapper roleManager, 
    IMapper mapper,
    IJwtService jwtService) : IAuthService
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
        
        var user = mapper.Map<ApplicationUser>(registerRequestDto);

        var result = await userManager.CreateAsync(user, registerRequestDto.Password);
        if (!result.Succeeded)
        {
            throw new  AuthException("User creation failed");
        }
        
        var roleSucceeded = await userManager.AddToRoleAsync(user, registerRequestDto.Role);
        if (!roleSucceeded.Succeeded)
        {
            throw new AuthException("Role assignment failed");       
        }
        
        var token = jwtService.GenerateToken(user);

        return new RegisterResponseDto
        {
            Id =  user.Id,
            AccessToken = token
        };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(loginRequestDto.Email, ct);
        if (user is null)
        {
            throw new AuthException("User not found");
        }
        var passwordCheckResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password, ct);
        if (!passwordCheckResult)
        {
            throw new AuthException("Password is invalid");
        }
        
        var token = jwtService.GenerateToken(user);
        return new LoginResponseDto()
        {
            Token = token
        };
    }
}