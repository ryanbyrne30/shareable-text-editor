using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BackendService.Gateway.Attributes;

public partial class PasswordValidationAttribute: ValidationAttribute
{
    private int MinLength { get; set; } = 8;
    private int MaxLength { get; set; } = 60;
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var password = value?.ToString();
        if (password == null) return new ValidationResult("Password is required.");
        if (password.Length < MinLength || password.Length > MaxLength) return new ValidationResult($"Password must be between {MinLength} and {MaxLength} characters long.");
        if (!PasswordRegex.UppercaseRegex().IsMatch(password)) return new ValidationResult("Password must contain at least one uppercase letter.");
        if (!PasswordRegex.LowercaseRegex().IsMatch(password)) return new ValidationResult("Password must contain at least one lowercase letter.");
        if (!PasswordRegex.NumberRegex().IsMatch(password)) return new ValidationResult("Password must contain at least one digit.");
        if (!PasswordRegex.SpecialRegex().IsMatch(password)) return new ValidationResult("Password must contain at least one special character.");
        return ValidationResult.Success;
    }
    
    protected static partial class PasswordRegex
    {
        [GeneratedRegex("[A-Z]+")]
        public static partial Regex UppercaseRegex();

        [GeneratedRegex("[a-z]+")]
        public static partial Regex LowercaseRegex(); 

        [GeneratedRegex("[@#$!%^&*()\\-_=+<>?/\"'`~{}[\\]\\\\]+")]
        public static partial Regex SpecialRegex(); 

        [GeneratedRegex("[0-9]+")]
        public static partial Regex NumberRegex(); 
    }
}