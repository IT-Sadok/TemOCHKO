using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Domain.Constants;
using FluentValidation;

namespace BookingManagerWeb.Application.Auth.Validators;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    private static readonly List<string> DefinedRoles = [ Roles.Client, Roles.User, Roles.Host ];

    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(9)
            .MaximumLength(40)
            .Matches("[a-z]").WithMessage("Password must have at least one lowercase characters")
            .Matches("[A-Z]").WithMessage("Password must have at least one uppercase characters")
            .Matches("[0-9]").WithMessage("Password must have at least one number");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(40);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(60);
        
        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(x => DefinedRoles.Contains(x))
            .WithMessage("Invalid role");
    }
}