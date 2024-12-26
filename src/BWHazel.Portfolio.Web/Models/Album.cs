using BWHazel.Data;

namespace BWHazel.Portfolio.Web.Models;

/// <summary>
/// Represents an album.
/// </summary>
/// <param name="AlbumId">The album ID.</param>
/// <param name="Title">The title.</param>
/// <param name="Description">The description.</param>
/// <param name="Year">The year.</param>
/// <param name="ImagePath">The album image path.</param>
public record Album
(
    StringId<Album> AlbumId,
    string Title,
    string Description,
    int Year,
    string ImagePath
);