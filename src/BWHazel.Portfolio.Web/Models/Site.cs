using BWHazel.Data;

namespace BWHazel.Portfolio.Web.Models;

/// <summary>
/// Represents a site in the Site List.
/// </summary>
/// <param name="Title">The title.</param>
/// <param name="Url">The URL.</param>
/// <param name="Description">The description.</param>
/// <param name="Hosting">The hosting location.</param>
/// <param name="SourceCodeUrl">The source code URL.</param>
/// <param name="Availability">The site availability.</param>
/// <param name="Notes">Notes about the site.</param>
public record Site
(
    StringId<Site> Title,
    string? Url,
    string Description,
    string Hosting,
    string? SourceCodeUrl,
    SiteAvailability Availability,
    string? Notes
);