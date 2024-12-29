using FluentValidation;

namespace BWHazel.Portfolio.Web.Models.Validators;

/// <summary>
/// Validator for the <see cref="Score"/> class.
/// </summary>
public class ScoreValidator : AbstractValidator<Score>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="ScoreValidator"/> class.
    /// </summary>
    public ScoreValidator()
    {
        this.RuleFor(score => score.ScoreId.Value)
            .NotEmpty()
            .MustBeKebabCase()
            .WithMessage("Score ID is required and must be in kebab case.");

        this.RuleFor(score => score.MuseScoreScoreId.Value)
            .GreaterThan(0)
            .WithMessage("MuseScore score ID is required and must be an integer.");

        this.RuleFor(score => score.Title)
            .NotEmpty()
            .WithMessage("Title is required.");

        this.RuleFor(score => score.Year)
            .GreaterThan(0)
            .WithMessage("Year is required and must be an integer.");

        this.RuleFor(score => score.Ensemble)
            .NotEmpty()
            .WithMessage("Ensemble is required.");

        this.RuleFor(score => score.Parts)
            .NotEmpty()
            .WithMessage("At least one part is required.");

        this.RuleForEach(score => score.Parts)
            .NotEmpty()
            .WithMessage("Parts must contain text.");

        this.RuleFor(score => score.ProgrammeNotes)
            .NotEmpty()
            .WithMessage("Programme notes are required.");
    }
}