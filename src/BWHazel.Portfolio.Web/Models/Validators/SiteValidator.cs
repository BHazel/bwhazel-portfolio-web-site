using FluentValidation;

namespace BWHazel.Portfolio.Web.Models.Validators;

/// <summary>
/// Validator for the <see cref="Site"/> class.
/// </summary>
public class SiteValidator : AbstractValidator<Site>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="SiteValidator"/> class.
    /// </summary>
    public SiteValidator()
    {
        this.RuleFor(site => site.Title.Value)
            .NotEmpty()
            .WithMessage("Title is required.");

        this.RuleFor(site => site.Url)
            .Uri(true)
            .WithMessage("Site URL is not valid.");

        this.RuleFor(site => site.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        this.RuleFor(site => site.Hosting)
            .NotEmpty()
            .WithMessage("Hosting is required.");
        
        this.RuleFor(site => site.SourceCodeUrl)
            .Uri(true)
            .WithMessage("Source code URL is not valid.");

        this.RuleFor(site => site.Availability)
            .NotEqual(SiteAvailability.Unknown)
            .WithMessage("Availability is required and must not be unknown.");
    }
}