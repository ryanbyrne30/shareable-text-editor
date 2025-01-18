using System.ComponentModel.DataAnnotations;
using DocumentService.Users.Attributes;

namespace BackendService_UnitTests.Users.Attributes;

public class PasswordValidationAttributeTest
{
    private readonly PasswordValidationAttribute _attribute = new();

    [Test]
    [TestCase("Valid1Password!")]
    [TestCase("Another1Valid@Password")]
    [TestCase("Strong1Password#")]
    [TestCase("Secure1Password$")]
    [TestCase("Complex1Password%")]
    [TestCase("Good1Password^")]
    [TestCase("Safe1Password&")]
    [TestCase("Protected1Password*")]
    [TestCase("Robust1Password(")]
    [TestCase("Reliable1Password)")]
    public void IsValid_ReturnsSuccess_ForValidPassword(string password)
    {
        var result = _attribute.GetValidationResult(password, new ValidationContext(new object()));
        Assert.That(result, Is.EqualTo(ValidationResult.Success));
    }

    [Test]
    [TestCase(null)]
    [TestCase("Short1!")]
    [TestCase("Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa1!")]
    [TestCase("nouppercase1!")]
    [TestCase("NOLOWERCASE1!")]
    [TestCase("NoDigitPassword!")]
    [TestCase("NoSpecial1Password")]
    public void IsValid_ReturnsError_ForNullPassword(string? password)
    {
        var result = _attribute.GetValidationResult(password, new ValidationContext(new object()));
        Assert.That(result, Is.Not.Null);
    }
}