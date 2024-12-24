using BWHazel.Data;

namespace BWHazel.Portfolio.Web.Models;

/// <summary>
/// Represents a musical score.
/// </summary>
/// <param name="ScoreId">The BWHazel Portfolio score ID.</param>
/// <param name="MuseScoreScoreId">The MuseScore score ID.</param>
/// <param name="Title">The title.</param>
/// <param name="Year">The year of composition.</param>
/// <param name="Ensemble">The ensemble.</param>
/// <param name="Parts">The parts.</param>
/// <param name="ProgrammeNotes">The programme notes.</param>
/// <remarks>This is based on a score hosted on MuseScore.</remarks>
public record Score
(
    StringId<Score> ScoreId,
    IntId<Score> MuseScoreScoreId,
    string Title,
    int Year,
    string Ensemble,
    string[] Parts,
    string ProgrammeNotes
);