using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using BWHazel.Portfolio.Web.Models;
using BWHazel.Portfolio.Web.Models.Validators;
using BWHazel.Portfolio.Web.Services;
using FluentValidation;
using Shouldly;
using Xunit;

namespace BWHazel.Portfolio.Web.Test.Services;

/// <summary>
/// Tests for the <see cref="AlbumService"/> class.
/// </summary>
public class AlbumServiceTests
{
    /// <summary>
    /// Tests the <see cref="AlbumService.GetAllAlbums"/> method returns all albums when present in configuration.
    /// </summary>
    [Fact]
    public void GetAllAlbums_WithExistingAlbums_ReturnsAllAlbums()
    {
        Dictionary<string, string?> albumConfiguration = new()
        {
            ["Music:Albums:Works:0:AlbumId"] = "album-1",
            ["Music:Albums:Works:0:Title"] = "Album 1",
            ["Music:Albums:Works:0:Description"] = "Album Description 1",
            ["Music:Albums:Works:0:Year"] = "2021",
            ["Music:Albums:Works:0:ImagePath"] = "img/music/albums/album-1.jpg",
            ["Music:Albums:Works:1:AlbumId"] = "album-2",
            ["Music:Albums:Works:1:Title"] = "Album 2",
            ["Music:Albums:Works:1:Description"] = "Album Description 2",
            ["Music:Albums:Works:1:Year"] = "2022",
            ["Music:Albums:Works:1:ImagePath"] = "img/music/albums/album-2.jpg"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(albumConfiguration)
            .Build();

        AlbumService albumService = new(configuration, new AlbumValidator());

        List<Album> albums = albumService.GetAllAlbums();

        albums.Count.ShouldBe(2);
        albums[0].AlbumId.Value.ShouldBe("album-1");
        albums[0].Title.ShouldBe("Album 1");
        albums[0].Description.ShouldBe("Album Description 1");
        albums[0].Year.ShouldBe(2021);
        albums[0].ImagePath.ShouldBe("img/music/albums/album-1.jpg");
        albums[1].AlbumId.Value.ShouldBe("album-2");
        albums[1].Title.ShouldBe("Album 2");
        albums[1].Description.ShouldBe("Album Description 2");
        albums[1].Year.ShouldBe(2022);
        albums[1].ImagePath.ShouldBe("img/music/albums/album-2.jpg");
    }

    /// <summary>
    /// Tests that the <see cref="AlbumService.GetAllAlbums"/> method returns an empty list when no albums are present in configuration.
    /// </summary>
    [Fact]
    public void GetAllAlbums_WithNoExistingAlbums_ReturnsEmptyList()
    {
        Dictionary<string, string?> albumConfiguration = [];
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(albumConfiguration)
            .Build();

        AlbumService albumService = new(configuration, new AlbumValidator());

        List<Album> albums = albumService.GetAllAlbums();

        albums.ShouldNotBeNull();
        albums.Count.ShouldBe(0);
    }

    /// <summary>
    /// Tests that the <see cref="AlbumService.GetAllAlbums"/> method throws an exception when the album ID is null.
    /// </summary>
    [Fact]
    public void GetAllAlbums_WithNullAlbumId_ThrowsException()
    {
        Dictionary<string, string?> albumConfiguration = new()
        {
            ["Music:Albums:Works:0:AlbumId"] = null,
            ["Music:Albums:Works:0:Title"] = "Album 1",
            ["Music:Albums:Works:0:Description"] = "Album Description 1",
            ["Music:Albums:Works:0:Year"] = "2021",
            ["Music:Albums:Works:0:ImagePath"] = "img/music/albums/album-1.jpg"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(albumConfiguration)
            .Build();

        AlbumService albumService = new(configuration, new AlbumValidator());

        ArgumentException exception = Should.Throw<ArgumentException>(() => albumService.GetAllAlbums());
        exception.Message.ShouldBe("Album ID is required.");
    }

    /// <summary>
    /// Tests that the <see cref="AlbumService.GetAllAlbums"/> method throws an exception when the album ID is invalid.
    /// </summary>
    /// <param name="invalidAlbumId">The invalid album ID.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("album id")]
    [InlineData(" album-id ")]
    public void GetAllAlbums_WithInvalidAlbumId_ThrowsException(string? invalidAlbumId)
    {
        Dictionary<string, string?> albumConfiguration = new()
        {
            ["Music:Albums:Works:0:AlbumId"] = invalidAlbumId,
            ["Music:Albums:Works:0:Title"] = "Album 1",
            ["Music:Albums:Works:0:Description"] = "Album Description 1",
            ["Music:Albums:Works:0:Year"] = "2021",
            ["Music:Albums:Works:0:ImagePath"] = "img/music/albums/album-1.jpg"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(albumConfiguration)
            .Build();

        AlbumService albumService = new(configuration, new AlbumValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => albumService.GetAllAlbums());
        exception.Message.ShouldContain("Album ID is required and must be in kebab case.");
    }

    /// <summary>
    /// Tests that the <see cref="AlbumService.GetAllAlbums"/> method throws an exception when the album title is invalid.
    /// </summary>
    /// <param name="invalidTitle">The invalid title.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void GetAllAlbums_WithInvalidTitle_ThrowsException(string? invalidTitle)
    {
        Dictionary<string, string?> albumConfiguration = new()
        {
            ["Music:Albums:Works:0:AlbumId"] = "album-1",
            ["Music:Albums:Works:0:Title"] = invalidTitle,
            ["Music:Albums:Works:0:Description"] = "Album Description 1",
            ["Music:Albums:Works:0:Year"] = "2021",
            ["Music:Albums:Works:0:ImagePath"] = "img/music/albums/album-1.jpg"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(albumConfiguration)
            .Build();

        AlbumService albumService = new(configuration, new AlbumValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => albumService.GetAllAlbums());
        exception.Message.ShouldContain("Title is required.");
    }

    /// <summary>
    /// Tests that the <see cref="AlbumService.GetAllAlbums"/> method throws an exception when the album description is invalid.
    /// </summary>
    /// <param name="invalidDescription">The invalid description.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void GetAllAlbums_WithInvalidDescription_ThrowsException(string? invalidDescription)
    {
        Dictionary<string, string?> albumConfiguration = new()
        {
            ["Music:Albums:Works:0:AlbumId"] = "album-1",
            ["Music:Albums:Works:0:Title"] = "Album 1",
            ["Music:Albums:Works:0:Description"] = invalidDescription,
            ["Music:Albums:Works:0:Year"] = "2021",
            ["Music:Albums:Works:0:ImagePath"] = "img/music/albums/album-1.jpg"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(albumConfiguration)
            .Build();

        AlbumService albumService = new(configuration, new AlbumValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => albumService.GetAllAlbums());
        exception.Message.ShouldContain("Description is required.");
    }

    /// <summary>
    /// Tests that the <see cref="AlbumService.GetAllAlbums"/> method throws an exception when the album year is invalid.
    /// </summary>
    /// <param name="invalidYear">The invalid year.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("abc")]
    [InlineData("0")]
    [InlineData("-1")]
    public void GetAllAlbums_WithInvalidYear_ThrowsException(string invalidYear)
    {
        Dictionary<string, string?> albumConfiguration = new()
        {
            ["Music:Albums:Works:0:AlbumId"] = "album-1",
            ["Music:Albums:Works:0:Title"] = "Album 1",
            ["Music:Albums:Works:0:Description"] = "Album Description 1",
            ["Music:Albums:Works:0:Year"] = invalidYear,
            ["Music:Albums:Works:0:ImagePath"] = "album-1.jpg"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(albumConfiguration)
            .Build();

        AlbumService albumService = new(configuration, new AlbumValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => albumService.GetAllAlbums());
        exception.Message.ShouldContain("Year is required and must be an integer.");
    }

    /// <summary>
    /// Tests that the <see cref="AlbumService.GetAllAlbums"/> method throws an exception when the album image path is invalid.
    /// </summary>
    /// <param name="invalidImagePath">The invalid image path.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("image.png")]
    [InlineData("/img/image.png")]
    [InlineData("img/image.png")]
    [InlineData("img/music/image.png")]
    public void GetAllAlbums_WithInvalidImagePath_ThrowsException(string invalidImagePath)
    {
        Dictionary<string, string?> albumConfiguration = new()
        {
            ["Music:Albums:Works:0:AlbumId"] = "album-1",
            ["Music:Albums:Works:0:Title"] = "Album 1",
            ["Music:Albums:Works:0:Description"] = "Album Description 1",
            ["Music:Albums:Works:0:Year"] = "2021",
            ["Music:Albums:Works:0:ImagePath"] = invalidImagePath,
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(albumConfiguration)
            .Build();

        AlbumService albumService = new(configuration, new AlbumValidator());

        ValidationException exception = Should.Throw<ValidationException>(() => albumService.GetAllAlbums());
        exception.Message.ShouldContain("Image path is required and must be in the 'img/music/albums' directory.");
    }
}