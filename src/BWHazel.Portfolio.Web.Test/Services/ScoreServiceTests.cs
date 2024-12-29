using System;
using System.Collections.Generic;
using System.Linq;
using BWHazel.Data;
using BWHazel.Portfolio.Web.Models;
using BWHazel.Portfolio.Web.Models.Validators;
using BWHazel.Portfolio.Web.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Shouldly;
using Xunit;

namespace BWHazel.Portfolio.Web.Test.Services;

/// <summary>
/// Tests for the <see cref="ScoreService"/> class.
/// </summary>
public class ScoreServiceTests
{
    /// <summary>
    /// Tests that the <see cref="ScoreService.GetMuseScoreUser"/> method returns a MuseScore user when present in configuration.
    /// </summary>
    [Fact]
    public void GetMuseScoreUser_WithExistingUser_ReturnsUser()
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:MuseScoreUser:Id"] = "123",
            ["Music:Scores:MuseScoreUser:Username"] = "ExampleUser"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        MuseScoreUser museScoreUser = scoreService.GetMuseScoreUser();

        museScoreUser.ShouldNotBeNull();
        museScoreUser.MuseScoreUserId.Value.ShouldBe(123);
        museScoreUser.Username.ShouldBe("ExampleUser");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetMuseScoreUser"/> method throws an exception when the MuseScore user ID is invalid.
    /// </summary>
    /// <param name="invalidMuseScoreUserId">The invalid MuseScore user ID.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("abc")]
    [InlineData("0")]
    [InlineData("-1")]
    public void GetMuseScoreUser_WithInvalidMuseScoreUserId_ThrowsException(string? invalidMuseScoreUserId)
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:MuseScoreUser:Id"] = invalidMuseScoreUserId,
            ["Music:Scores:MuseScoreUser:Username"] = "ExampleUser"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => scoreService.GetMuseScoreUser());
        exception.Message.ShouldContain("MuseScore user ID is required and must be an integer.");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetMuseScoreUser"/> method throws an exception when the MuseScore username is invalid.
    /// </summary>
    /// <param name="invalidMuseScoreUsername">The invalid MuseScore username.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void GetMuseScoreUser_WithInvalidUsername_ThrowsException(string? invalidMuseScoreUsername)
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:MuseScoreUser:Id"] = "123",
            ["Music:Scores:MuseScoreUser:Username"] = invalidMuseScoreUsername
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => scoreService.GetMuseScoreUser());
        exception.Message.ShouldContain("Username is required.");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetAllScores"/> method returns all scores when present in configuration.
    /// </summary>
    [Fact]
    public void GetAllScores_WithExistingScores_ReturnsAllScores()
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:Works:0:ScoreId"] = "score-1",
            ["Music:Scores:Works:0:MuseScoreScoreId"] = "1001",
            ["Music:Scores:Works:0:Title"] = "Score 1",
            ["Music:Scores:Works:0:Year"] = "2021",
            ["Music:Scores:Works:0:Ensemble"] = "Score Ensemble 1",
            ["Music:Scores:Works:0:Parts:0"] = "Score 1 Part 1",
            ["Music:Scores:Works:0:Parts:1"] = "Score 1 Part 2",
            ["Music:Scores:Works:0:ProgrammeNotes"] = "Score 1 Programme Notes",
            ["Music:Scores:Works:1:ScoreId"] = "score-2",
            ["Music:Scores:Works:1:MuseScoreScoreId"] = "1002",
            ["Music:Scores:Works:1:Title"] = "Score 2",
            ["Music:Scores:Works:1:Year"] = "2022",
            ["Music:Scores:Works:1:Ensemble"] = "Score Ensemble 2",
            ["Music:Scores:Works:1:Parts:0"] = "Score 2 Part 1",
            ["Music:Scores:Works:1:Parts:1"] = "Score 2 Part 2",
            ["Music:Scores:Works:1:ProgrammeNotes"] = "Score 2 Programme Notes"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        List<Score> scores = scoreService.GetAllScores();

        scores.Count.ShouldBe(2);
        scores[0].ScoreId.Value.ShouldBe("score-1");
        scores[0].MuseScoreScoreId.Value.ShouldBe(1001);
        scores[0].Title.ShouldBe("Score 1");
        scores[0].Year.ShouldBe(2021);
        scores[0].Ensemble.ShouldBe("Score Ensemble 1");
        scores[0].Parts.Length.ShouldBe(2);
        scores[0].Parts.ElementAt(0).ShouldBe("Score 1 Part 1");
        scores[0].Parts.ElementAt(1).ShouldBe("Score 1 Part 2");
        scores[0].ProgrammeNotes.ShouldBe("Score 1 Programme Notes");
        scores[1].ScoreId.Value.ShouldBe("score-2");
        scores[1].MuseScoreScoreId.Value.ShouldBe(1002);
        scores[1].Title.ShouldBe("Score 2");
        scores[1].Year.ShouldBe(2022);
        scores[1].Ensemble.ShouldBe("Score Ensemble 2");
        scores[1].Parts.Length.ShouldBe(2);
        scores[1].Parts.ElementAt(0).ShouldBe("Score 2 Part 1");
        scores[1].Parts.ElementAt(1).ShouldBe("Score 2 Part 2");
        scores[1].ProgrammeNotes.ShouldBe("Score 2 Programme Notes");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetAllScores"/> method returns an empty list when no scores are present in configuration.
    /// </summary>
    [Fact]
    public void GetAllScores_WithNoExistingScores_ReturnsEmptyList()
    {
        Dictionary<string, string?> scoreConfiguration = [];
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        List<Score> scores = scoreService.GetAllScores();

        scores.ShouldNotBeNull();
        scores.Count.ShouldBe(0);
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetAllScores"/> method throws an exception when the score ID is null.
    /// </summary>
    [Fact]
    public void GetAllScores_WithNullScoreId_ThrowsException()
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:Works:0:ScoreId"] = null,
            ["Music:Scores:Works:0:MuseScoreScoreId"] = "1001",
            ["Music:Scores:Works:0:Title"] = "Score 1",
            ["Music:Scores:Works:0:Year"] = "2021",
            ["Music:Scores:Works:0:Ensemble"] = "Score Ensemble 1",
            ["Music:Scores:Works:0:Parts:0"] = "Score 1 Part 1",
            ["Music:Scores:Works:0:Parts:1"] = "Score 1 Part 2",
            ["Music:Scores:Works:0:ProgrammeNotes"] = "Score 1 Programme Notes"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        ArgumentException exception = Should.Throw<ArgumentException>(() => scoreService.GetAllScores());
        exception.Message.ShouldBe("Score ID is required.");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetAllScores"/> method throws an exception when the score ID is invalid.
    /// </summary>
    /// <param name="invalidScoreId">The invalid score ID.</param>
    [Theory]
    [InlineData(" ")]
    [InlineData("score id")]
    [InlineData(" score-id ")]
    public void GetAllScores_WithInvalidScoreId_ThrowsException(string? invalidScoreId)
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:Works:0:ScoreId"] = invalidScoreId,
            ["Music:Scores:Works:0:MuseScoreScoreId"] = "1001",
            ["Music:Scores:Works:0:Title"] = "Score 1",
            ["Music:Scores:Works:0:Year"] = "2021",
            ["Music:Scores:Works:0:Ensemble"] = "Score Ensemble 1",
            ["Music:Scores:Works:0:Parts:0"] = "Score 1 Part 1",
            ["Music:Scores:Works:0:Parts:1"] = "Score 1 Part 2",
            ["Music:Scores:Works:0:ProgrammeNotes"] = "Score 1 Programme Notes"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => scoreService.GetAllScores());
        exception.Message.ShouldContain("Score ID is required and must be in kebab case.");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetAllScores"/> method throws an exception when the MuseScore score ID is invalid.
    /// </summary>
    /// <param name="invalidMuseScoreScoreId">The invalid MuseScore score ID.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("abc")]
    [InlineData("0")]
    [InlineData("-1")]
    public void GetAllScores_WithInvalidMuseScoreScoreId_ThrowsException(string? invalidMuseScoreScoreId)
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:Works:0:ScoreId"] = "score-1",
            ["Music:Scores:Works:0:MuseScoreScoreId"] = invalidMuseScoreScoreId,
            ["Music:Scores:Works:0:Title"] = "Score 1",
            ["Music:Scores:Works:0:Year"] = "2021",
            ["Music:Scores:Works:0:Ensemble"] = "Score Ensemble 1",
            ["Music:Scores:Works:0:Parts:0"] = "Score 1 Part 1",
            ["Music:Scores:Works:0:Parts:1"] = "Score 1 Part 2",
            ["Music:Scores:Works:0:ProgrammeNotes"] = "Score 1 Programme Notes"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => scoreService.GetAllScores());
        exception.Message.ShouldContain("MuseScore score ID is required and must be an integer.");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetAllScores"/> method throws an exception when the score title is invalid.
    /// </summary>
    /// <param name="invalidTitle">The invalid title.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void GetAllScores_WithInvalidTitle_ThrowsException(string? invalidTitle)
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:Works:0:ScoreId"] = "score-1",
            ["Music:Scores:Works:0:MuseScoreScoreId"] = "1001",
            ["Music:Scores:Works:0:Title"] = invalidTitle,
            ["Music:Scores:Works:0:Year"] = "2021",
            ["Music:Scores:Works:0:Ensemble"] = "Score Ensemble 1",
            ["Music:Scores:Works:0:Parts:0"] = "Score 1 Part 1",
            ["Music:Scores:Works:0:Parts:1"] = "Score 1 Part 2",
            ["Music:Scores:Works:0:ProgrammeNotes"] = "Score 1 Programme Notes"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => scoreService.GetAllScores());
        exception.Message.ShouldContain("Title is required.");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetAllScores"/> method throws an exception when the score year is invalid.
    /// </summary>
    /// <param name="invalidYear">The invalid year.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("abc")]
    [InlineData("0")]
    [InlineData("-1")]
    public void GetAllScores_WithInvalidYear_ThrowsException(string? invalidYear)
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:Works:0:ScoreId"] = "score-1",
            ["Music:Scores:Works:0:MuseScoreScoreId"] = "1001",
            ["Music:Scores:Works:0:Title"] = "Score 1",
            ["Music:Scores:Works:0:Year"] = invalidYear,
            ["Music:Scores:Works:0:Ensemble"] = "Score Ensemble 1",
            ["Music:Scores:Works:0:Parts:0"] = "Score 1 Part 1",
            ["Music:Scores:Works:0:Parts:1"] = "Score 1 Part 2",
            ["Music:Scores:Works:0:ProgrammeNotes"] = "Score 1 Programme Notes"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => scoreService.GetAllScores());
        exception.Message.ShouldContain("Year is required and must be an integer.");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetAllScores"/> method throws an exception when the score ensemble is invalid.
    /// </summary>
    /// <param name="invalidEnsemble">The invalid ensemble.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void GetAllScores_WithInvalidEnsemble_ThrowsException(string? invalidEnsemble)
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:Works:0:ScoreId"] = "score-1",
            ["Music:Scores:Works:0:MuseScoreScoreId"] = "1001",
            ["Music:Scores:Works:0:Title"] = "Score 1",
            ["Music:Scores:Works:0:Year"] = "2021",
            ["Music:Scores:Works:0:Ensemble"] = invalidEnsemble,
            ["Music:Scores:Works:0:Parts:0"] = "Score 1 Part 1",
            ["Music:Scores:Works:0:Parts:1"] = "Score 1 Part 2",
            ["Music:Scores:Works:0:ProgrammeNotes"] = "Score 1 Programme Notes"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => scoreService.GetAllScores());
        exception.Message.ShouldContain("Ensemble is required.");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetAllScores"/> method throws an exception no score parts are present.
    /// </summary>
    [Fact]
    public void GetAllScores_WithNoParts_ThrowsException()
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:Works:0:ScoreId"] = "score-1",
            ["Music:Scores:Works:0:MuseScoreScoreId"] = "1001",
            ["Music:Scores:Works:0:Title"] = "Score 1",
            ["Music:Scores:Works:0:Year"] = "2021",
            ["Music:Scores:Works:0:Ensemble"] = "Score Ensemble 1",
            ["Music:Scores:Works:0:ProgrammeNotes"] = "Score 1 Programme Notes"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => scoreService.GetAllScores());
        exception.Message.ShouldContain("At least one part is required.");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetAllScores"/> method throws an exception when a score part is invalid.
    /// </summary>
    /// <param name="invalidPart">The invalid part.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void GetAllScores_WithInvalidParts_ThrowsException(string? invalidPart)
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:Works:0:ScoreId"] = "score-1",
            ["Music:Scores:Works:0:MuseScoreScoreId"] = "1001",
            ["Music:Scores:Works:0:Title"] = "Score 1",
            ["Music:Scores:Works:0:Year"] = "2021",
            ["Music:Scores:Works:0:Ensemble"] = "Score 1 Ensemble",
            ["Music:Scores:Works:0:Parts:0"] = invalidPart,
            ["Music:Scores:Works:0:ProgrammeNotes"] = "Score 1 Programme Notes"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => scoreService.GetAllScores());
        exception.Message.ShouldContain("Parts must contain text.");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetAllScores"/> method throws an exception when the score programme notes are invalid.
    /// </summary>
    /// <param name="invalidProgrammeNotes">The invalid programme notes.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void GetAllScores_WithInvalidProgrammeNotes_ThrowsException(string? invalidProgrammeNotes)
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:Works:0:ScoreId"] = "score-1",
            ["Music:Scores:Works:0:MuseScoreScoreId"] = "1001",
            ["Music:Scores:Works:0:Title"] = "Score 1",
            ["Music:Scores:Works:0:Year"] = "2021",
            ["Music:Scores:Works:0:Ensemble"] = "Score Ensemble 1",
            ["Music:Scores:Works:0:Parts:0"] = "Score 1 Part 1",
            ["Music:Scores:Works:0:Parts:1"] = "Score 1 Part 2",
            ["Music:Scores:Works:0:ProgrammeNotes"] = invalidProgrammeNotes
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => scoreService.GetAllScores());
        exception.Message.ShouldContain("Programme notes are required.");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetScore"/> method returns a score when the score ID matches a score in configuration.
    /// </summary>
    [Fact]
    public void GetScore_WithScoreIdForExistingScore_ReturnsScore()
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:Works:0:ScoreId"] = "score-1",
            ["Music:Scores:Works:0:MuseScoreScoreId"] = "1001",
            ["Music:Scores:Works:0:Title"] = "Score 1",
            ["Music:Scores:Works:0:Year"] = "2021",
            ["Music:Scores:Works:0:Ensemble"] = "Score Ensemble 1",
            ["Music:Scores:Works:0:Parts:0"] = "Score 1 Part 1",
            ["Music:Scores:Works:0:Parts:1"] = "Score 1 Part 2",
            ["Music:Scores:Works:0:ProgrammeNotes"] = "Score 1 Programme Notes"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        Score? score = scoreService.GetScore(new("score-1"));

        score.ShouldNotBeNull();
        score!.ScoreId.Value.ShouldBe("score-1");
        score!.MuseScoreScoreId.Value.ShouldBe(1001);
        score!.Title.ShouldBe("Score 1");
        score!.Year.ShouldBe(2021);
        score!.Ensemble.ShouldBe("Score Ensemble 1");
        score!.Parts.Length.ShouldBe(2);
        score!.Parts.ElementAt(0).ShouldBe("Score 1 Part 1");
        score!.Parts.ElementAt(1).ShouldBe("Score 1 Part 2");
        score!.ProgrammeNotes.ShouldBe("Score 1 Programme Notes");
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetScore"/> method returns null when the score ID does not match a score in configuration.
    /// </summary>
    [Fact]
    public void GetScore_WithScoreIdForNonExistingScore_ReturnsNull()
    {
        Dictionary<string, string?> scoreConfiguration = new()
        {
            ["Music:Scores:Works:0:ScoreId"] = "score-1",
            ["Music:Scores:Works:0:MuseScoreScoreId"] = "1001",
            ["Music:Scores:Works:0:Title"] = "Score 1",
            ["Music:Scores:Works:0:Year"] = "2021",
            ["Music:Scores:Works:0:Ensemble"] = "Score Ensemble 1",
            ["Music:Scores:Works:0:Parts:0"] = "Score 1 Part 1",
            ["Music:Scores:Works:0:Parts:1"] = "Score 1 Part 2",
            ["Music:Scores:Works:0:ProgrammeNotes"] = "Score 1 Programme Notes"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        Score? score = scoreService.GetScore(new("score-2"));

        score.ShouldBeNull();
    }

    /// <summary>
    /// Tests that the <see cref="ScoreService.GetScore"/> method throws an exception when the score ID is empty.
    /// </summary>
    [Fact]
    public void GetScore_WithEmptyScoreId_ThrowsException()
    {
        Dictionary<string, string?> scoreConfiguration = [];
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(scoreConfiguration)
            .Build();

        ScoreService scoreService = new(configuration, new ScoreValidator(), new MuseScoreUserValidator());

        ArgumentException exception = Should.Throw<ArgumentException>(() => scoreService.GetScore(new(string.Empty)));
        exception.Message.ShouldBe("Score ID is required.");
    }
}