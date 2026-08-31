using BookingManagerWeb.Application.Auth.DTOs;
using FluentValidation;

namespace BookingManagerWeb.Application.Auth.Validators;

public class LoginRequestDtoValidator: AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid Email");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}