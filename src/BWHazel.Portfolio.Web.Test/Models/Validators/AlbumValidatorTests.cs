using BWHazel.Portfolio.Web.Models;
using BWHazel.Portfolio.Web.Models.Validators;
using FluentValidation.TestHelper;
using Shouldly;
using Xunit;

namespace BWHazel.Portfolio.Web.Test.Models.Validators;

/// <summary>
/// Tests for the <see cref="AlbumValidator"/> class.
/// </summary>
public class AlbumValidatorTests
{
    /// <summary>
    /// Tests that the <see cref="AlbumValidator"/> class returns a valid result for a valid album.
    /// </summary>
    [Fact]
    public void Validate_WithValidAlbum_ReturnsValidResult()
    {
        AlbumValidator validator = new();
        Album album = new(
            "album-1",
            "Album 1",
            "Album 1 Description",
            2021,
            "img/music/albums/album-1.png");

        TestValidationResult<Album> validationResult = validator.TestValidate(album);

        validationResult.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that the <see cref="AlbumValidator"/> class returns an invalid result for an album with an invalid album ID.
    /// </summary>
    /// <param name="invalidAlbumId">The invalid album ID.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("album id")]
    [InlineData(" album-id ")]
    public void Validate_WithInvalidAlbumId_ReturnsInvalidResult(string invalidAlbumId)
    {
        AlbumValidator validator = new();
        Album album = new(
            invalidAlbumId,
            "Album 1",
            "Album 1 Description",
            2021,
            "img/music/albums/album-1.png");

        TestValidationResult<Album> validationResult = validator.TestValidate(album);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(a => a.AlbumId.Value);
    }

    /// <summary>
    /// Tests that the <see cref="AlbumValidator"/> class returns an invalid result for an album with an invalid title.
    /// </summary>
    /// <param name="invalidTitle">The invalid title.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidTitle_ReturnsInvalidResult(string invalidTitle)
    {
        AlbumValidator validator = new();
        Album album = new(
            "album-1",
            invalidTitle,
            "Album 1 Description",
            2021,
            "img/music/albums/album-1.png");

        TestValidationResult<Album> validationResult = validator.TestValidate(album);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(a => a.Title);
    }
    
    /// <summary>
    /// Tests that the <see cref="AlbumValidator"/> class returns an invalid result for an album with an invalid description.
    /// </summary>
    /// <param name="invalidDescription">The invalid description.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidDescription_ReturnsInvalidResult(string invalidDescription)
    {
        AlbumValidator validator = new();
        Album album = new(
            "album-1",
            "Album 1",
            invalidDescription,
            2021,
            "img/music/albums/album-1.png");

        TestValidationResult<Album> validationResult = validator.TestValidate(album);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(a => a.Description);
    }
    
    /// <summary>
    /// Tests that the <see cref="AlbumValidator"/> class returns an invalid result for an album with an invalid year.
    /// </summary>
    /// <param name="invalidYear">The invalid year.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithInvalidYear_ReturnsInvalidResult(int invalidYear)
    {
        AlbumValidator validator = new();
        Album album = new(
            "album-1",
            "Album 1",
            "Album 1 Description",
            invalidYear,
            "img/music/albums/album-1.png");

        TestValidationResult<Album> validationResult = validator.TestValidate(album);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(a => a.Year);
    }
    
    /// <summary>
    /// Tests that the <see cref="AlbumValidator"/> class returns an invalid result for an album with an invalid image path.
    /// </summary>
    /// <param name="invalidImagePath">The invalid image path.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("image.png")]
    [InlineData("/img/image.png")]
    [InlineData("img/image.png")]
    [InlineData("img/music/image.png")]
    public void Validate_WithInvalidImagePath_ReturnsInvalidResult(string invalidImagePath)
    {
        AlbumValidator validator = new();
        Album album = new(
            "album-1",
            "Album 1",
            "Album 1 Description",
            2021,
            invalidImagePath);

        TestValidationResult<Album> validationResult = validator.TestValidate(album);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(a => a.ImagePath);
    }
}