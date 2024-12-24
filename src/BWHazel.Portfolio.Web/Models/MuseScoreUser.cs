using BWHazel.Data;

namespace BWHazel.Portfolio.Web.Models;

/// <summary>
/// Represents a MuseScore user.
/// </summary>
/// <param name="MuseScoreUserId">The MuseScore user ID.</param>
/// <param name="Username">The username.</param>
public record MuseScoreUser
(
    IntId<MuseScoreUser> MuseScoreUserId, 
    string Username
);