using System.ComponentModel.DataAnnotations;

namespace BackendService.Gateway.Attributes;

public partial class PageValidationAttribute: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;
        var page = (int) value;
        return page > 0 ? ValidationResult.Success : new ValidationResult("Page must be greater than 0.");
    }
}