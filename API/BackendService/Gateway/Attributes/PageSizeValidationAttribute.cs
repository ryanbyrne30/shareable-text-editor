using System.ComponentModel.DataAnnotations;

namespace BackendService.Gateway.Attributes;

public partial class PageSizeValidationAttribute: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;
        var pageSize = (int) value;
        return pageSize > 0 & pageSize <= 100 ? ValidationResult.Success : new ValidationResult("Page size must be between 1 and 100.");
    }
}