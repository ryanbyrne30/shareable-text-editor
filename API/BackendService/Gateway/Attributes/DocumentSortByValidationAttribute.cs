using System.ComponentModel.DataAnnotations;

namespace BackendService.Gateway.Attributes;

public partial class DocumentSortByValidationAttribute: ValidationAttribute
{
    public static readonly Dictionary<string, string> ValidValues = new () 
    {
        { "name", "Name" },
        { "updated_at", "UpdatedAt" },
        { "created_at", "CreatedAt" }
    };
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var sortBy = value?.ToString();
        if (sortBy == null) return ValidationResult.Success;
        var values = ValidValues.Keys;
        return values.Contains(sortBy) ? ValidationResult.Success : new ValidationResult($"Sort by must be one of the following: {string.Join(", ", values)}.");
    }
}