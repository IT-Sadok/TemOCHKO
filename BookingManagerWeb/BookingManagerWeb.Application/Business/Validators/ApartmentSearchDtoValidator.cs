using BookingManagerWeb.Application.Business.DTOs;
using FluentValidation;

namespace BookingManagerWeb.Application.Business.Validators;

public class ApartmentSearchDtoValidator : AbstractValidator<ApartmentSearchDto>
{
    public ApartmentSearchDtoValidator()
    {
        RuleFor(x => x.CheckIn)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today)).When(x => x.CheckIn.HasValue && x.CheckOut.HasValue)
            .WithMessage("Check-in must be in the future")
            .LessThan(dto => dto.CheckOut).When(x => x.CheckIn.HasValue && x.CheckOut.HasValue)
            .WithMessage("Check in must be before check out time");
        
        RuleFor(x => x.CheckOut)
            .GreaterThan(dto => dto.CheckIn).When(x => x.CheckIn.HasValue && x.CheckOut.HasValue)
            .WithMessage("Check Out time must be after Check In time");

        RuleFor(x => x.Guests)
            .GreaterThan(0).When(x => x.Guests.HasValue)
            .WithMessage("Guests must be greater than 0");
    }
}