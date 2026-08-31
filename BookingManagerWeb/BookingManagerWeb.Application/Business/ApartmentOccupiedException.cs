using BookingManagerWeb.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BookingManagerWeb.Application.Business;

public class ApartmentOccupiedException(string message) 
    : AppBaseException(message, StatusCodes.Status400BadRequest, "Apartment is occupied for these dates");
