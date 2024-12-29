using BWHazel.Portfolio.Web.Models;
using BWHazel.Portfolio.Web.Models.Validators;
using FluentValidation.TestHelper;
using Shouldly;
using Xunit;

namespace BWHazel.Portfolio.Web.Test.Models.Validators;

/// <summary>
/// Tests for the <see cref="MuseScoreUserValidator"/> class.
/// </summary>
public class MuseScoreUserValidatorTests
{
    /// <summary>
    /// Tests that the <see cref="MuseScoreUserValidator"/> class returns a valid result for a valid MuseScore user.
    /// </summary>
    [Fact]
    public void Validate_WithValidMuseScoreUser_ReturnsValidResult()
    {
        MuseScoreUserValidator validator = new();
        MuseScoreUser museScoreUser = new(123, "ExampleUser");

        TestValidationResult<MuseScoreUser> validationResult = validator.TestValidate(museScoreUser);

        validationResult.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that the <see cref="MuseScoreUserValidator"/> class returns an invalid result for a MuseScore user with and invalid MuseScore user ID.
    /// </summary>
    /// <param name="invalidMuseScoreUserId">The invalid MuseScore user ID.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithInvalidMuseScoreUserId_ReturnsInvalidResult(int invalidMuseScoreUserId)
    {
        MuseScoreUserValidator validator = new();
        MuseScoreUser museScoreUser = new(invalidMuseScoreUserId, "ExampleUser");

        TestValidationResult<MuseScoreUser> validationResult = validator.TestValidate(museScoreUser);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(m => m.MuseScoreUserId.Value);
    }
    
    /// <summary>
    /// Tests that the <see cref="MuseScoreUserValidator"/> class returns an invalid result for a MuseScore user with an invalid username.
    /// </summary>
    /// <param name="invalidMuseScoreUsername">The invalid MuseScore username.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidUsername_ReturnsInvalidResult(string invalidMuseScoreUsername)
    {
        MuseScoreUserValidator validator = new();
        MuseScoreUser museScoreUser = new(123, invalidMuseScoreUsername);

        TestValidationResult<MuseScoreUser> validationResult = validator.TestValidate(museScoreUser);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(m => m.Username);
    }
}