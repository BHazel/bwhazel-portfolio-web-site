using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using BWHazel.Portfolio.Web.Models;
using FluentValidation;

namespace BWHazel.Portfolio.Web.Services;

/// <summary>
/// Works with albums.
/// </summary>
/// <param name="configuration">The application configuration.</param>
/// <param name="albumValidator">The album validator.</param>
public class AlbumService(IConfiguration configuration, IValidator<Album> albumValidator)
{
    private const string MusicAlbumsWorksKey = "Music:Albums:Works";

    private readonly IConfiguration configuration = configuration;
    private readonly IValidator<Album> albumValidator = albumValidator;

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
    private Album MapConfigurationToAlbumInfo(IConfigurationSection musicAlbumsWorksSection)
    {
        Album album = new(
            musicAlbumsWorksSection["AlbumId"]!
                ?? throw new ArgumentException("Album ID is required."),
            musicAlbumsWorksSection["Title"]!,
            musicAlbumsWorksSection["Description"]!,
            int.TryParse(musicAlbumsWorksSection["Year"]!, out int year)
                ? year
                : 0,
            musicAlbumsWorksSection["ImagePath"]!
        );

        this.albumValidator.ValidateAndThrow(album);
        return album;
    }
}