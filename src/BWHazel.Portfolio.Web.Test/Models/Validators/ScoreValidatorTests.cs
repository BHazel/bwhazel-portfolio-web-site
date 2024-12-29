using BWHazel.Portfolio.Web.Models;
using BWHazel.Portfolio.Web.Models.Validators;
using FluentValidation.TestHelper;
using Shouldly;
using Xunit;

namespace BWHazel.Portfolio.Web.Test.Models.Validators;

/// <summary>
/// Tests for the <see cref="ScoreValidator"/> class.
/// </summary>
public class ScoreValidatorTests
{
    /// <summary>
    /// Tests that the <see cref="ScoreValidator"/> class returns a valid result for a valid score.
    /// </summary>
    [Fact]
    public void Validate_WithValidScore_ReturnsValidResult()
    {
        ScoreValidator validator = new();
        Score score = new(
            "score-1",
            1001,
            "Score 1",
            2021,
            "Score Ensemble 1",
            ["Score 1 Part 1", "Score 1 Part 2"],
            "Score 1 Programme Notes");

        TestValidationResult<Score> validationResult = validator.TestValidate(score);

        validationResult.IsValid.ShouldBeTrue();
    }
    
    /// <summary>
    /// Tests that the <see cref="ScoreValidator"/> class returns an invalid result for a score with an invalid score ID.
    /// </summary>
    /// <param name="invalidScoreId">The invalid score ID.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("score id")]
    [InlineData(" score-id ")]
    public void Validate_WithInvalidScoreId_ReturnsInvalidResult(string invalidScoreId)
    {
        ScoreValidator validator = new();
        Score score = new(
            invalidScoreId,
            1001,
            "Score 1",
            2021,
            "Score Ensemble 1",
            ["Score 1 Part 1", "Score 1 Part 2"],
            "Score 1 Programme Notes");

        TestValidationResult<Score> validationResult = validator.TestValidate(score);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(s => s.ScoreId.Value);
    }
    
    /// <summary>
    /// Tests that the <see cref="ScoreValidator"/> class returns an invalid result for a score with an invalid MuseScore score ID.
    /// </summary>
    /// <param name="invalidMuseScoreScoreId">The invalid MuseScore score ID.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithInvalidMuseScoreScoreId_ReturnsInvalidResult(int invalidMuseScoreScoreId)
    {
        ScoreValidator validator = new();
        Score score = new(
            "score-1",
            invalidMuseScoreScoreId,
            "Score 1",
            2021,
            "Score Ensemble 1",
            ["Score 1 Part 1", "Score 1 Part 2"],
            "Score 1 Programme Notes");

        TestValidationResult<Score> validationResult = validator.TestValidate(score);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(s => s.MuseScoreScoreId.Value);
    }
    
    /// <summary>
    /// Tests that the <see cref="ScoreValidator"/> class returns an invalid result for a score with an invalid title.
    /// </summary>
    /// <param name="invalidTitle">The invalid title.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidTitle_ReturnsInvalidResult(string invalidTitle)
    {
        ScoreValidator validator = new();
        Score score = new(
            "score-1",
            1001,
            invalidTitle,
            2021,
            "Score Ensemble 1",
            ["Score 1 Part 1", "Score 1 Part 2"],
            "Score 1 Programme Notes");

        TestValidationResult<Score> validationResult = validator.TestValidate(score);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(s => s.Title);
    }
    
    /// <summary>
    /// Tests that the <see cref="ScoreValidator"/> class returns an invalid result for a score with an invalid year.
    /// </summary>
    /// <param name="invalidYear">The invalid year.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithInvalidYearReturnsInvalidResult(int invalidYear)
    {
        ScoreValidator validator = new();
        Score score = new(
            "score-1",
            1001,
            "Score 1",
            invalidYear,
            "Score Ensemble 1",
            ["Score 1 Part 1", "Score 1 Part 2"],
            "Score 1 Programme Notes");

        TestValidationResult<Score> validationResult = validator.TestValidate(score);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(s => s.Year);
    }
    
    /// <summary>
    /// Tests that the <see cref="ScoreValidator"/> class returns an invalid result for a score with an invalid ensemble.
    /// </summary>
    /// <param name="invalidEnsemble">The invalid ensemble.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidEnsemble_ReturnsInvalidResult(string invalidEnsemble)
    {
        ScoreValidator validator = new();
        Score score = new(
            "score-1",
            1001,
            "Score 1",
            2021,
            invalidEnsemble,
            ["Score 1 Part 1", "Score 1 Part 2"],
            "Score 1 Programme Notes");

        TestValidationResult<Score> validationResult = validator.TestValidate(score);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(s => s.Ensemble);
    }
    
    /// <summary>
    /// Tests that the <see cref="ScoreValidator"/> class returns an invalid result for a score with no parts.
    /// </summary>
    [Fact]
    public void Validate_WithNoParts_ReturnsInvalidResult()
    {
        ScoreValidator validator = new();
        Score score = new(
            "score-1",
            1001,
            "Score 1",
            2021,
            "Score Ensemble 1",
            [],
            "Score 1 Programme Notes");

        TestValidationResult<Score> validationResult = validator.TestValidate(score);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(s => s.Parts);
    }
    
    /// <summary>
    /// Tests that the <see cref="ScoreValidator"/> class returns an invalid result for a score with an invalid part.
    /// </summary>
    /// <param name="invalidPart">The invalid part.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidParts_ReturnsInvalidResult(string invalidPart)
    {
        ScoreValidator validator = new();
        Score score = new(
            "score-1",
            1001,
            "Score 1",
            2021,
            "Score Ensemble 1",
            [invalidPart],
            "Score 1 Programme Notes");

        TestValidationResult<Score> validationResult = validator.TestValidate(score);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(s => s.Parts);
    }
    
    /// <summary>
    /// Tests that the <see cref="ScoreValidator"/> class returns an invalid result for a score with invalid programme notes.
    /// </summary>
    /// <param name="invalidProgrammeNotes">The invalid programme notes.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidProgrammeNotes_ReturnsInvalidResult(string invalidProgrammeNotes)
    {
        ScoreValidator validator = new();
        Score score = new(
            "score-1",
            1001,
            "Score 1",
            2021,
            "Score 1 Ensemble",
            ["Score 1 Part 1", "Score 1 Part 2"],
            invalidProgrammeNotes);

        TestValidationResult<Score> validationResult = validator.TestValidate(score);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.ShouldHaveValidationErrorFor(s => s.ProgrammeNotes);
    }
}