using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using BWHazel.Data;
using BWHazel.Portfolio.Web.Models;

namespace BWHazel.Portfolio.Web.Services;

/// <summary>
/// Works with musical scores and MuseScore users.
/// </summary>
/// <param name="configuration">The application configuration.</param>
public class ScoreService(IConfiguration configuration)
{
    private const string MusicScoresMuseScoreUserKey = "Music:Scores:MuseScoreUser";
    private const string MusicScoresWorksKey = "Music:Scores:Works";
    
    private readonly IConfiguration configuration = configuration;
    
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
    public List<Score> GetScores()
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
        List<Score> scores = this.GetScores();
        Score? score = scores.FirstOrDefault(s => s.ScoreId == scoreId);
        return score;
    }
    
    /// <summary>
    /// Maps a configuration section to a MuseScore user.
    /// </summary>
    /// <param name="musicScoresMuseScoreUserSection">The configuration section.</param>
    /// <returns>A MuseScore user.</returns>
    private static MuseScoreUser MapConfigurationToMuseScoreUser(IConfigurationSection musicScoresMuseScoreUserSection)
    {
        MuseScoreUser museScoreUser = new(
            int.Parse(musicScoresMuseScoreUserSection["Id"]!),
            musicScoresMuseScoreUserSection["Username"]!
        );
        
        return museScoreUser;
    }
    
    /// <summary>
    /// Maps a configuration section to a score.
    /// </summary>
    /// <param name="musicScoreWorkSection">The configuration section.</param>
    /// <returns>A score.</returns>
    private static Score MapConfigurationToScore(IConfigurationSection musicScoreWorkSection)
    {
        Score score = new(
            musicScoreWorkSection["ScoreId"]!,
            int.Parse(musicScoreWorkSection["MuseScoreScoreId"]!),
            musicScoreWorkSection["Title"]!,
            int.Parse(musicScoreWorkSection["Year"]!),
            musicScoreWorkSection["Ensemble"]!,
            musicScoreWorkSection.GetSection("Parts").Get<string[]>()!,
            musicScoreWorkSection["ProgrammeNotes"]!
        );

        return score;
    }
}