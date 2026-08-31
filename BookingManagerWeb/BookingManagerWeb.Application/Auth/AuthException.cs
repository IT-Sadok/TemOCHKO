using BookingManagerWeb.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BookingManagerWeb.Application.Auth;

public class AuthException(string message)
    : AppBaseException(message, StatusCodes.Status400BadRequest, "Authentication error");