using FluentValidation;

namespace BWHazel.Portfolio.Web.Models.Validators;

/// <summary>
/// Validates the <see cref="MuseScoreUser"/> class.
/// </summary>
public class MuseScoreUserValidator : AbstractValidator<MuseScoreUser>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="MuseScoreUserValidator"/> class.
    /// </summary>
    public MuseScoreUserValidator()
    {
        this.RuleFor(museScoreUser => museScoreUser.MuseScoreUserId.Value)
            .GreaterThan(0)
            .WithMessage("MuseScore user ID is required and must be an integer.");

        this.RuleFor(museScoreUser => museScoreUser.Username)
            .NotEmpty()
            .WithMessage("MuseScore username is required.");
    }
}