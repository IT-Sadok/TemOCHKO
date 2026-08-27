using BookingManagerWeb.Application.Business.DTO_s;
using FluentValidation;

namespace BookingManagerWeb.Application.Business.Validators;

public class BookingSearchDtoValidator : AbstractValidator<BookingCreateDto>
{
    public BookingSearchDtoValidator()
    {
        RuleFor(x => x.ApartmentId)
            .NotEmpty().WithMessage("Apartment Id is required");
        
        RuleFor(x => x.StartDate)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Check-in must be in the future")
            .LessThan(dto => dto.EndDate)
            .WithMessage("Check in must be before check out time");
        
        RuleFor(x => x.EndDate)
            .GreaterThan(dto => dto.StartDate)
            .WithMessage("Check Out time must be after Check In time");
    }
}