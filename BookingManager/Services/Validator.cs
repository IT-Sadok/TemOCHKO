using System.Text.RegularExpressions;
using Models;
using Models.DTOs;

namespace Services;

public static class Validator
{
    public record struct ValidationError(string errorMessage, string memberName);

    public static List<ValidationError> Validate(this HostCreateDTO hostCreateDto)
    {
        var errors = new List<ValidationError>();
        errors.AddRange(ValidateHost(hostCreateDto.FirstName,  hostCreateDto.LastName, hostCreateDto.Type, hostCreateDto.Email, hostCreateDto.Phone, hostCreateDto.DateOfBirth));
        return errors;
    }

    public static List<ValidationError> Validate(this HostDetailsDTO hostDetailsDto)
    {
        var errors = new List<ValidationError>();
        errors.AddRange(ValidateHost(hostDetailsDto.FirstName, hostDetailsDto.LastName, hostDetailsDto.Type, hostDetailsDto.Email, hostDetailsDto.Phone, hostDetailsDto.DateOfBirth));
        return errors;
    }

    public static List<ValidationError> ValidateHost(string firstName, string lastName, HostType hostType, string email,
        string phone, DateTime dateOfBirth)
    {
        var errors = new List<ValidationError>();
        errors.AddRange(ValidateHostName(firstName, nameof(HostCreateDTO.FirstName), "First Name"));
        errors.AddRange(ValidateHostName(lastName, nameof(HostCreateDTO.LastName), "Last Name"));
        if (IsNameReserved(firstName + " " + lastName))
            errors.Add(new ValidationError($"Full Name must not be a reserved word (exit, back etc.)", nameof(HostCreateDTO.FirstName) + " " + nameof(HostCreateDTO.LastName)));
        errors.AddRange(ValidateEmail(email, nameof(HostCreateDTO.Email), "Email"));
        errors.AddRange(ValidatePhone(phone, nameof(HostCreateDTO.Phone), "Phone"));
        errors.AddRange(ValidateDateOfBirth(dateOfBirth, nameof(HostCreateDTO.DateOfBirth), "Date of birth"));
        return errors;
    }
    
    private static List<ValidationError> ValidateHostName(string name, string propertyName, string displayName)
    {
        var errors = new List<ValidationError>();
        if (String.IsNullOrWhiteSpace(name))
        {
            errors.Add(new ValidationError($"{displayName} can't be empty.", propertyName));
            return errors;
        }
        if (name.Length < 2)
            errors.Add(new ValidationError($"{displayName} must be at least 2 characters long.", propertyName));
        if (!(!string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[\p{L}\s]+$")))
            errors.Add(new ValidationError($"{displayName} must consist only from letters.", propertyName));
        return errors;
    }
    
    private static List<ValidationError> ValidateDateOfBirth(DateTime? date, string propertyName, string displayName)
    {
        var errors = new List<ValidationError>();
        if (date == null)
            errors.Add(new ValidationError($"{displayName}  must be selected.", propertyName));
        if (date >= new DateTime(DateTime.Today.Year - 18, DateTime.Today.Month, DateTime.Today.Day))
            errors.Add(new ValidationError($"Host can't be less than 18 years old. ", propertyName));
        return errors;
    }

    private static List<ValidationError> ValidateEmail(string email, string propertyName, string displayName)
    {
        var errors = new List<ValidationError>();
        if (String.IsNullOrWhiteSpace(email))
            errors.Add(new ValidationError($"{displayName} can't be empty.", propertyName));
        if (email.Length < 15)
            errors.Add(new ValidationError($"{displayName} must be at least 15 characters long.", propertyName));
        if (!email.Contains("@gmail.com") && !email.Contains("@ukr.net"))
            errors.Add(new ValidationError($"{displayName} must be a valid email address.", propertyName));
        return errors;
    }

    private static List<ValidationError> ValidatePhone(string phone, string propertyName, string displayName)
    {
        var errors = new List<ValidationError>();
        if (String.IsNullOrWhiteSpace(phone))
            errors.Add(new ValidationError($"{displayName} can't be empty.", propertyName));
        if (phone.Length < 9)
            errors.Add(new ValidationError($"{displayName} must be at least 9 characters long.", propertyName));
        return errors;
    }
    
    private static bool IsNameReserved(string fullName)
    {
        fullName = fullName.ToLower();
        fullName =  fullName.Trim();
        switch (fullName)
        {
            case "remove host":
                return true;
            case "add host":
                return true;
            case "save changes":
                return true;
            default:
                return false;
        }
    }
}