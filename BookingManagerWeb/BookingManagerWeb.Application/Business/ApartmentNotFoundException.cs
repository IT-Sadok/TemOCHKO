using BookingManagerWeb.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BookingManagerWeb.Application.Business;

public class ApartmentNotFoundException(string message)
    : AppBaseException(message, StatusCodes.Status404NotFound, "Apartment not found");