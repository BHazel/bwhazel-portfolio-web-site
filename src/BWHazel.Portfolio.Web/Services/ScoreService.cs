using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using BWHazel.Data;
using BWHazel.Portfolio.Web.Models;
using FluentValidation;

namespace BWHazel.Portfolio.Web.Services;

/// <summary>
/// Works with musical scores and MuseScore users.
/// </summary>
/// <param name="configuration">The application configuration.</param>
/// <param name="scoreValidator">The score validator.</param>
/// <param name="museScoreUserValidator">The MuseScore user validator.</param>
public class ScoreService(IConfiguration configuration, IValidator<Score> scoreValidator, IValidator<MuseScoreUser> museScoreUserValidator)
{
    private const string MusicScoresMuseScoreUserKey = "Music:Scores:MuseScoreUser";
    private const string MusicScoresWorksKey = "Music:Scores:Works";

    private readonly IConfiguration configuration = configuration;
    private readonly IValidator<Score> scoreValidator = scoreValidator;
    private readonly IValidator<MuseScoreUser> museScoreUserValidator = museScoreUserValidator;

    /// <summary>
    /// Gets the MuseScore user.
    /// </summary>
    /// <returns>The MuseScore user.</returns>
    public MuseScoreUser GetMuseScoreUser()
    {
        IConfigurationSection musicScoresMuseScoreUserSection = this.configuration.GetSection(MusicScoresMuseScoreUserKey);
        MuseScoreUser museScoreUser = MapConfigurationToMuseScoreUser(musicScoresMuseScoreUserSection);
        return museScoreUser;
    }

    /// <summary>
    /// Gets all scores.
    /// </summary>
    /// <returns>A collection of all scores.</returns>
    public List<Score> GetAllScores()
    {
        IConfigurationSection musicScoresWorksSection = this.configuration.GetSection(MusicScoresWorksKey);
        List<Score> scores = musicScoresWorksSection
            .GetChildren()
            .Select(MapConfigurationToScore)
            .ToList();

        return scores;
    }

    /// <summary>
    /// Gets a score.
    /// </summary>
    /// <param name="scoreId">The score ID.</param>
    /// <returns>A score.</returns>
    public Score? GetScore(StringId<Score> scoreId)
    {
        if (string.IsNullOrWhiteSpace(scoreId.Value))
        {
            throw new ArgumentException("Score ID is required.");
        }

        List<Score> scores = this.GetAllScores();
        Score? score = scores.FirstOrDefault(s => s.ScoreId == scoreId);
        return score;
    }

    /// <summary>
    /// Maps a configuration section to a MuseScore user.
    /// </summary>
    /// <param name="musicScoresMuseScoreUserSection">The configuration section.</param>
    /// <returns>A MuseScore user.</returns>
    private MuseScoreUser MapConfigurationToMuseScoreUser(IConfigurationSection musicScoresMuseScoreUserSection)
    {
        MuseScoreUser museScoreUser = new(
            int.TryParse(musicScoresMuseScoreUserSection["Id"]!, out int museScoreUserId) ? museScoreUserId : 0,
            musicScoresMuseScoreUserSection["Username"]!
        );

        this.museScoreUserValidator.ValidateAndThrow(museScoreUser);
        return museScoreUser;
    }

    /// <summary>
    /// Maps a configuration section to a score.
    /// </summary>
    /// <param name="musicScoreWorkSection">The configuration section.</param>
    /// <returns>A score.</returns>
    private Score MapConfigurationToScore(IConfigurationSection musicScoreWorkSection)
    {
        Score score = new(
            musicScoreWorkSection["ScoreId"]!
                ?? throw new ArgumentException("Score ID is required."),
            int.TryParse(musicScoreWorkSection["MuseScoreScoreId"]!, out int museScoreScoreId) ? museScoreScoreId : 0,
            musicScoreWorkSection["Title"]!,
            int.TryParse(musicScoreWorkSection["Year"]!, out int year) ? year : 0,
            musicScoreWorkSection["Ensemble"]!,
            musicScoreWorkSection.GetSection("Parts").Get<string[]>()!,
            musicScoreWorkSection["ProgrammeNotes"]!
        );

        this.scoreValidator.ValidateAndThrow(score);
        return score;
    }
}