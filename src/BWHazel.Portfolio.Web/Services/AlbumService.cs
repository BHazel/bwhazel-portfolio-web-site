using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using BWHazel.Portfolio.Web.Models;

namespace BWHazel.Portfolio.Web.Services;

/// <summary>
/// Works with albums.
/// </summary>
public class AlbumService(IConfiguration configuration)
{
    private const string MusicAlbumsWorksKey = "Music:Albums:Works";

    private readonly IConfiguration configuration = configuration;

    /// <summary>
    /// Gets all albums.
    /// </summary>
    /// <returns>A collection of all albums.</returns>
    public List<Album> GetAllAlbums()
    {
        IConfigurationSection musicAlbumsWorksSection = this.configuration.GetSection(MusicAlbumsWorksKey);
        List<Album> albums = musicAlbumsWorksSection
            .GetChildren()
            .Select(MapConfigurationToAlbumInfo)
            .ToList();

        return albums;
    }

    /// <summary>
    /// Maps a configuration section to an album summary.
    /// </summary>
    /// <param name="musicAlbumsWorksSection">The configuration section.</param>
    /// <returns>An album summary.</returns>
    private static Album MapConfigurationToAlbumInfo(IConfigurationSection musicAlbumsWorksSection)
    {
        Album album = new(
            musicAlbumsWorksSection["AlbumId"]!,
            musicAlbumsWorksSection["Title"]!,
            musicAlbumsWorksSection["Description"]!,
            int.Parse(musicAlbumsWorksSection["Year"]!),
            musicAlbumsWorksSection["ImagePath"]!
        );

        return album;
    }
}