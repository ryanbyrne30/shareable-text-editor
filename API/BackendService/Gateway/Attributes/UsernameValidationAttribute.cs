using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BackendService.Gateway.Attributes;

public partial class UsernameValidationAttribute: ValidationAttribute
{
    private int MinLength { get; set; } = 3;
    private int MaxLength { get; set; } = 20;
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var username = value?.ToString();
        if (username == null) return new ValidationResult("Username is required.");
        if (username.Length < MinLength || username.Length > MaxLength) return new ValidationResult($"Username must be between {MinLength} and {MaxLength} characters long.");
        if (!UsernameRegex.ValidCharacters().IsMatch(username)) return new ValidationResult("Username can only contain letters, numbers, dashes, underscores, and periods.");
        return ValidationResult.Success;
    }
    
    private static partial class UsernameRegex
    {
        [GeneratedRegex("^[a-zA-Z0-9-_.]+$")]
        public static partial Regex ValidCharacters();
    }
    
}