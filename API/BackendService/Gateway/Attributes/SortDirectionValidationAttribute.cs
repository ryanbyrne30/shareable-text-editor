using System.ComponentModel.DataAnnotations;

namespace BackendService.Gateway.Attributes;

public partial class SortDirectionValidationAttribute: ValidationAttribute
{
    private static readonly List<string> ValidValues = ["asc", "desc"];
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var sortBy = value?.ToString();
        if (sortBy == null) return ValidationResult.Success;
        return ValidValues.Contains(sortBy) ? ValidationResult.Success : new ValidationResult($"Sort by must be one of the following: {string.Join(", ", ValidValues)}.");
    }
}