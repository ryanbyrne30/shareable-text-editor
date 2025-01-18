using System.ComponentModel.DataAnnotations;
using BackendService.Gateway.Attributes;

namespace BackendService_UnitTests.Users.Attributes;

public class UsernameValidationAttributeTest
{
    private readonly UsernameValidationAttribute _attribute = new();
    
    [Test]
    [TestCase("user123")]
    [TestCase("john_doe")]
    [TestCase("jane.doe")]
    [TestCase("user-name")]
    [TestCase("validUser1")]
    [TestCase("another_user")]
    [TestCase("user.name")]
    [TestCase("user-name123")]
    [TestCase("user_123")]
    [TestCase("username")]
    public void IsValid_ReturnsSuccess_ForValidUsername(string username)
    {
        var result = _attribute.GetValidationResult(username, new ValidationContext(new object()));
        Assert.That(result, Is.EqualTo(ValidationResult.Success));
    }
    
    [Test]
    [TestCase("")]
    [TestCase("a")]
    [TestCase("ab")]
    [TestCase("aaaaaaaaaaaaaaaaaaaaa")]
    [TestCase("user@name")]
    [TestCase("user!name")]
    [TestCase("user name")]
    [TestCase("user#name")]
    [TestCase("user$name")]
    [TestCase("user%name")]
    public void IsInvalid_ReturnsFail(string username)
    {
        var result = _attribute.GetValidationResult(username, new ValidationContext(new object()));
        Assert.That(result, Is.Not.Null);
    }
}