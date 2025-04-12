using BWHazel.Portfolio.Web.Models;
using BWHazel.Portfolio.Web.Models.Validators;
using FluentValidation.TestHelper;
using Shouldly;
using Xunit;

namespace BWHazel.Portfolio.Web.Test.Models.Validators;

/// <summary>
/// Tests for the <see cref="SiteValidator"/> class.
/// </summary>
public class SiteValidatorTests
{
    private readonly Site site = new(
        "My Site",
        "https://example.co.uk",
        "My Site Description",
        "My Site Hosting",
        "https://source.example.co.uk",
        SiteAvailability.AlwaysAvailable,
        "My Site Notes");

    /// <summary>
    /// Tests that the <see cref="SiteValidator"/> class returns a valid result for a valid site.
    /// </summary>
    [Fact]
    public void Validate_WithValidSite_ReturnsValidResult()
    {
        SiteValidator validator = new();

        TestValidationResult<Site> validationResult = validator.TestValidate(this.site);

        validationResult.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that the <see cref="SiteValidator"/> class returns an invalid result for a site with an invalid title.
    /// </summary>
    /// <param name="invalidTitle">The invalid title.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidTitle_ReturnsInvalidResult(string invalidTitle)
    {
        SiteValidator validator = new();
        Site updatedSite = this.site with { Title = invalidTitle };

        TestValidationResult<Site> validationResult = validator.TestValidate(updatedSite);

        validationResult.IsValid.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that the <see cref="SiteValidator"/> class returns a valid result for a site with a missing URL.
    /// </summary>
    [Fact]
    public void Validate_WithMissingUrl_ReturnsValidResult()
    {
        SiteValidator validator = new();
        Site updatedSite = this.site with { Url = null };

        TestValidationResult<Site> validationResult = validator.TestValidate(updatedSite);

        validationResult.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that the <see cref="SiteValidator"/> class returns an invalid result for a site with an invalid URL.
    /// </summary>
    /// <param name="invalidUrl">The invalid URL.</param>
    [Theory]
    [InlineData("abc")]
    [InlineData("abc.co.uk")]
    [InlineData("https:/example")]
    public void Validate_WithInvalidUrl_ReturnsInvalidResult(string invalidUrl)
    {
        SiteValidator validator = new();
        Site updatedSite = this.site with { Url = invalidUrl };

        TestValidationResult<Site> validationResult = validator.TestValidate(updatedSite);

        validationResult.IsValid.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that the <see cref="SiteValidator"/> class returns an invalid result for a site with an invalid description.
    /// </summary>
    /// <param name="invalidDescription">The invalid description.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidDescription_ReturnsInvalidResult(string invalidDescription)
    {
        SiteValidator validator = new();
        Site updatedSite = this.site with { Description = invalidDescription };

        TestValidationResult<Site> validationResult = validator.TestValidate(updatedSite);

        validationResult.IsValid.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that the <see cref="SiteValidator"/> class returns an invalid result for a site with an invalid hosting.
    /// </summary>
    /// <param name="invalidHosting">The invalid hosting.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidHosting_ReturnsInvalidResult(string invalidHosting)
    {
        SiteValidator validator = new();
        Site updatedSite = this.site with { Hosting = invalidHosting };

        TestValidationResult<Site> validationResult = validator.TestValidate(updatedSite);

        validationResult.IsValid.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that the <see cref="SiteValidator"/> class returns a valid result for a site with a missing source code URL.
    /// </summary>
    [Fact]
    public void Validate_WithMissingSourceCodeUrl_ReturnsValidResult()
    {
        SiteValidator validator = new();
        Site updatedSite = this.site with { SourceCodeUrl = null };

        TestValidationResult<Site> validationResult = validator.TestValidate(updatedSite);

        validationResult.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that the <see cref="SiteValidator"/> class returns an invalid result for a site with an invalid source code URL.
    /// </summary>
    /// <param name="invalidSourceCodeUrl">The invalid source code URL.</param>
    [Theory]
    [InlineData("abc")]
    [InlineData("abc.co.uk")]
    [InlineData("https:/example")]
    public void Validate_WithInvaliSourceCodedUrl_ReturnsInvalidResult(string invalidSourceCodeUrl)
    {
        SiteValidator validator = new();
        Site updatedSite = this.site with { SourceCodeUrl = invalidSourceCodeUrl };

        TestValidationResult<Site> validationResult = validator.TestValidate(updatedSite);

        validationResult.IsValid.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that the <see cref="SiteValidator"/> class returns an invalid result for a site with an unknown availability.
    /// </summary>
    [Fact]
    public void Validate_WithUnknownSiteAvailability_ReturnsInvalidResult()
    {
        SiteValidator validator = new();
        Site updatedSite = this.site with { Availability = SiteAvailability.Unknown };

        TestValidationResult<Site> validationResult = validator.TestValidate(updatedSite);

        validationResult.IsValid.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that the <see cref="SiteValidator"/> class returns a valid result for a site with missing notes.
    /// </summary>
    [Fact]
    public void Validate_WithMissingNotes_ReturnsValidResult()
    {
        SiteValidator validator = new();
        Site updatedSite = this.site with { Notes = null };

        TestValidationResult<Site> validationResult = validator.TestValidate(updatedSite);

        validationResult.IsValid.ShouldBeTrue();
    }
}