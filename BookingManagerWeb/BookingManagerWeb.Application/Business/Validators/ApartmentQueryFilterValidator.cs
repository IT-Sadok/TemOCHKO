using BookingManagerWeb.Application.Business.DTOs.Pagination;
using FluentValidation;

namespace BookingManagerWeb.Application.Business.Validators;

public class ApartmentQueryFilterValidator : AbstractValidator<ApartmentQueryFilter>
{
    public ApartmentQueryFilterValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);
        
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1);
    }
}