using FluentValidation;

namespace BWHazel.Portfolio.Web.Models.Validators;

/// <summary>
/// Validator for the <see cref="Album"/> class.
/// </summary>
public class AlbumValidator : AbstractValidator<Album>
{
    private const string ImagePathRoot = "img/";
    private const string ImagePathPrefix = "music/albums";

    /// <summary>
    /// Initialises a new instance of the <see cref="AlbumValidator"/> class.
    /// </summary>
    public AlbumValidator()
    {
        this.RuleFor(album => album.AlbumId.Value)
            .NotEmpty()
            .MustBeKebabCase()
            .WithMessage("Album ID is required and must be in kebab case.");

        this.RuleFor(album => album.Title)
            .NotEmpty()
            .WithMessage("Title is required.");

        this.RuleFor(album => album.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        this.RuleFor(album => album.Year)
            .GreaterThan(0)
            .WithMessage("Year is required and must be an integer.");

        this.RuleFor(album => album.ImagePath)
            .NotEmpty()
            .MustBeImagePath(ImagePathPrefix)
            .WithMessage($"Image path is required and must be in the '{ImagePathRoot}{ImagePathPrefix}' directory.");
    }
}