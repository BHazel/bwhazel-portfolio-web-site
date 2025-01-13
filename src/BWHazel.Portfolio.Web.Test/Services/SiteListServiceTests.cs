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
/// Tests for the <see cref="SiteListService"/> class.
/// </summary>
public class SiteListServiceTests
{
    /// <summary>
    /// Tests that the <see cref="SiteListService.GetAllSites" /> method returns all sites when present in configuration.
    /// </summary>
    [Fact]
    public void GetAllSites_WithExistingSites_ReturnsAllSites()
    {
        Dictionary<string, string?> siteConfiguration = new()
        {
            ["SiteList:Sites:0:Title"] = "Site 1",
            ["SiteList:Sites:0:Url"] = "https://example1.co.uk",
            ["SiteList:Sites:0:Description"] = "Site 1 Description",
            ["SiteList:Sites:0:Hosting"] = "Site 1 Hosting",
            ["SiteList:Sites:0:SourceCodeUrl"] = "https://source.example1.co.uk",
            ["SiteList:Sites:0:Availability"] = "AlwaysAvailable",
            ["SiteList:Sites:0:Notes"] = "Site 1 Notes",
            ["SiteList:Sites:1:Title"] = "Site 2",
            ["SiteList:Sites:1:Url"] = "https://example2.co.uk",
            ["SiteList:Sites:1:Description"] = "Site 2 Description",
            ["SiteList:Sites:1:Hosting"] = "Site 2 Hosting",
            ["SiteList:Sites:1:SourceCodeUrl"] = "https://source.example2.co.uk",
            ["SiteList:Sites:1:Availability"] = "LimitedAccess",
            ["SiteList:Sites:1:Notes"] = "Site 2 Notes"
        };
        
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(siteConfiguration)
            .Build();
        
        SiteListService siteListService = new(configuration, new SiteValidator());

        List<Site> sites = siteListService.GetAllSites();
        
        sites.Count.ShouldBe(2);
        sites[0].Title.Value.ShouldBe("Site 1");
        sites[0].Url.ShouldBe("https://example1.co.uk");
        sites[0].Description.ShouldBe("Site 1 Description");
        sites[0].Hosting.ShouldBe("Site 1 Hosting");
        sites[0].SourceCodeUrl.ShouldBe("https://source.example1.co.uk");
        sites[0].Availability.ShouldBe(SiteAvailability.AlwaysAvailable);
        sites[0].Notes.ShouldBe("Site 1 Notes");
        sites[1].Title.Value.ShouldBe("Site 2");
        sites[1].Url.ShouldBe("https://example2.co.uk");
        sites[1].Description.ShouldBe("Site 2 Description");
        sites[1].Hosting.ShouldBe("Site 2 Hosting");
        sites[1].SourceCodeUrl.ShouldBe("https://source.example2.co.uk");
        sites[1].Availability.ShouldBe(SiteAvailability.LimitedAccess);
        sites[1].Notes.ShouldBe("Site 2 Notes");
    }

    /// <summary>
    /// Tests that the <see cref="SiteListService.GetAllSites" /> method returns an empty list when no sites are present in configuration.
    /// </summary>
    [Fact]
    public void GetAllSites_WithNoExistingSites_ReturnsEmptyList()
    {
        Dictionary<string, string?> siteConfiguration = [];
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(siteConfiguration)
            .Build();
        
        SiteListService siteListService = new(configuration, new SiteValidator());
        
        List<Site> sites = siteListService.GetAllSites();
        
        sites.ShouldNotBeNull();
        sites.Count.ShouldBe(0);
    }

    /// <summary>
    /// Tests that the <see cref="SiteListService.GetAllSites" /> method throws an exception when the title is invalid.
    /// </summary>
    /// <param name="invalidTitle">The invalid title.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void GetAllSites_WithInvalidTitle_ThrowsException(string? invalidTitle)
    {
        Dictionary<string, string?> siteConfiguration = new()
        {
            ["SiteList:Sites:0:Title"] = invalidTitle,
            ["SiteList:Sites:0:Url"] = "https://example1.co.uk",
            ["SiteList:Sites:0:Description"] = "Site 1 Description",
            ["SiteList:Sites:0:Hosting"] = "Site 1 Hosting",
            ["SiteList:Sites:0:SourceCodeUrl"] = "https://source.example1.co.uk",
            ["SiteList:Sites:0:Availability"] = "AlwaysAvailable",
            ["SiteList:Sites:0:Notes"] = "Site 1 Notes",
        };
        
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(siteConfiguration)
            .Build();
        
        SiteListService siteListService = new(configuration, new SiteValidator());
        
        ValidationException exception = Should.Throw<ValidationException>(() => siteListService.GetAllSites());
        exception.Message.ShouldContain("Title is required.");
    }

    /// <summary>
    /// Tests that the <see cref="SiteListService.GetAllSites" /> method returns all sites when the URL is not provided.
    /// </summary>
    [Fact]
    public void GetAllSites_WithMissingUrl_ReturnsAllSites()
    {
        Dictionary<string, string?> siteConfiguration = new()
        {
            ["SiteList:Sites:0:Title"] = "Site 1",
            ["SiteList:Sites:0:Url"] = null,
            ["SiteList:Sites:0:Description"] = "Site 1 Description",
            ["SiteList:Sites:0:Hosting"] = "Site 1 Hosting",
            ["SiteList:Sites:0:SourceCodeUrl"] = "https://source.example1.co.uk",
            ["SiteList:Sites:0:Availability"] = "AlwaysAvailable",
            ["SiteList:Sites:0:Notes"] = "Site 1 Notes",
        };
        
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(siteConfiguration)
            .Build();
        
        SiteListService siteListService = new(configuration, new SiteValidator());
        
        List<Site> sites = siteListService.GetAllSites();
        
        sites.Count.ShouldBe(1);
    }

    /// <summary>
    /// Tests that the <see cref="SiteListService.GetAllSites" /> method throws an exception when the URL is invalid.
    /// </summary>
    /// <param name="invalidUrl">The invalid URL.</param>
    [Theory]
    [InlineData("abc")]
    [InlineData("abc.co.uk")]
    [InlineData("https:/example")]
    public void GetAllSites_WithInvalidUrl_ThrowsException(string? invalidUrl)
    {
        Dictionary<string, string?> siteConfiguration = new()
        {
            ["SiteList:Sites:0:Title"] = "Site 1",
            ["SiteList:Sites:0:Url"] = invalidUrl,
            ["SiteList:Sites:0:Description"] = "Site 1 Description",
            ["SiteList:Sites:0:Hosting"] = "Site 1 Hosting",
            ["SiteList:Sites:0:SourceCodeUrl"] = "https://source.example1.co.uk",
            ["SiteList:Sites:0:Availability"] = "AlwaysAvailable",
            ["SiteList:Sites:0:Notes"] = "Site 1 Notes",
        };
        
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(siteConfiguration)
            .Build();
        
        SiteListService siteListService = new(configuration, new SiteValidator());
        
        ValidationException exception = Should.Throw<ValidationException>(() => siteListService.GetAllSites());
        exception.Message.ShouldContain("Site URL is not valid.");
    }
    
    /// <summary>
    /// Tests that the <see cref="SiteListService.GetAllSites" /> method throws an exception when the description is invalid.
    /// </summary>
    /// <param name="invalidDescription">The invalid description.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void GetAllSites_WithInvalidDescription_ThrowsException(string? invalidDescription)
    {
        Dictionary<string, string?> siteConfiguration = new()
        {
            ["SiteList:Sites:0:Title"] = "Site 1",
            ["SiteList:Sites:0:Url"] = "https://example1.co.uk",
            ["SiteList:Sites:0:Description"] = invalidDescription,
            ["SiteList:Sites:0:Hosting"] = "Site 1 Hosting",
            ["SiteList:Sites:0:SourceCodeUrl"] = "https://source.example1.co.uk",
            ["SiteList:Sites:0:Availability"] = "AlwaysAvailable",
            ["SiteList:Sites:0:Notes"] = "Site 1 Notes",
        };
        
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(siteConfiguration)
            .Build();
        
        SiteListService siteListService = new(configuration, new SiteValidator());
        
        ValidationException exception = Should.Throw<ValidationException>(() => siteListService.GetAllSites());
        exception.Message.ShouldContain("Description is required.");
    }
    
    /// <summary>
    /// Tests that the <see cref="SiteListService.GetAllSites" /> method throws an exception when the hosting is invalid.
    /// </summary>
    /// <param name="invalidHosting">The invalid hosting.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void GetAllSites_WithInvalidHosting_ThrowsException(string? invalidHosting)
    {
        Dictionary<string, string?> siteConfiguration = new()
        {
            ["SiteList:Sites:0:Title"] = "Site 1",
            ["SiteList:Sites:0:Url"] = "https://example1.co.uk",
            ["SiteList:Sites:0:Description"] = "Site 1 Description",
            ["SiteList:Sites:0:Hosting"] = invalidHosting,
            ["SiteList:Sites:0:SourceCodeUrl"] = "https://source.example1.co.uk",
            ["SiteList:Sites:0:Availability"] = "AlwaysAvailable",
            ["SiteList:Sites:0:Notes"] = "Site 1 Notes",
        };
        
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(siteConfiguration)
            .Build();
        
        SiteListService siteListService = new(configuration, new SiteValidator());
        
        ValidationException exception = Should.Throw<ValidationException>(() => siteListService.GetAllSites());
        exception.Message.ShouldContain("Hosting is required.");
    }
    
    /// <summary>
    /// Tests that the <see cref="SiteListService.GetAllSites" /> method returns all sites when the source code URL is not provided.
    /// </summary>
    [Fact]
    public void GetAllSites_WithMissingSourceCodeUrl_ReturnsAllSites()
    {
        Dictionary<string, string?> siteConfiguration = new()
        {
            ["SiteList:Sites:0:Title"] = "Site 1",
            ["SiteList:Sites:0:Url"] = "https://example1.co.uk",
            ["SiteList:Sites:0:Description"] = "Site 1 Description",
            ["SiteList:Sites:0:Hosting"] = "Site 1 Hosting",
            ["SiteList:Sites:0:SourceCodeUrl"] = null,
            ["SiteList:Sites:0:Availability"] = "AlwaysAvailable",
            ["SiteList:Sites:0:Notes"] = "Site 1 Notes",
        };
        
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(siteConfiguration)
            .Build();
        
        SiteListService siteListService = new(configuration, new SiteValidator());
        
        List<Site> sites = siteListService.GetAllSites();
        
        sites.Count.ShouldBe(1);
    }
    
    /// <summary>
    /// Tests that the <see cref="SiteListService.GetAllSites" /> method throws an exception when the source code URL is invalid.
    /// </summary>
    /// <param name="invalidSourceCodeUrl">The invalid source code URL.</param>
    [Theory]
    [InlineData("abc")]
    [InlineData("abc.co.uk")]
    [InlineData("https:/example")]
    public void GetAllSites_WithInvalidSourceCodeUrl_ThrowsException(string? invalidSourceCodeUrl)
    {
        Dictionary<string, string?> siteConfiguration = new()
        {
            ["SiteList:Sites:0:Title"] = "Site 1",
            ["SiteList:Sites:0:Url"] = "https://example1.co.uk",
            ["SiteList:Sites:0:Description"] = "Site 1 Description",
            ["SiteList:Sites:0:Hosting"] = "Site 1 Hosting",
            ["SiteList:Sites:0:SourceCodeUrl"] = invalidSourceCodeUrl,
            ["SiteList:Sites:0:Availability"] = "AlwaysAvailable",
            ["SiteList:Sites:0:Notes"] = "Site 1 Notes",
        };
        
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(siteConfiguration)
            .Build();
        
        SiteListService siteListService = new(configuration, new SiteValidator());
        
        ValidationException exception = Should.Throw<ValidationException>(() => siteListService.GetAllSites());
        exception.Message.ShouldContain("Source code URL is not valid.");
    }

    /// <summary>
    /// Tests that the <see cref="SiteListService.GetAllSites" /> method throws an exception when the availability is invalid.
    /// </summary>
    /// <param name="invalidAvailability">The invalid availability.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("abc")]
    [InlineData("Unknown")]
    [InlineData(null)]
    public void GetAllSites_WithInvalidAvailability_ThrowsException(string? invalidAvailability)
    {
        Dictionary<string, string?> siteConfiguration = new()
        {
            ["SiteList:Sites:0:Title"] = "Site 1",
            ["SiteList:Sites:0:Url"] = "https://example1.co.uk",
            ["SiteList:Sites:0:Description"] = "Site 1 Description",
            ["SiteList:Sites:0:Hosting"] = "Site 1 Hosting",
            ["SiteList:Sites:0:SourceCodeUrl"] = "https://source.example1.co.uk",
            ["SiteList:Sites:0:Availability"] = invalidAvailability,
            ["SiteList:Sites:0:Notes"] = "Site 1 Notes",
        };
        
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(siteConfiguration)
            .Build();
        
        SiteListService siteListService = new(configuration, new SiteValidator());
        
        ValidationException exception = Should.Throw<ValidationException>(() => siteListService.GetAllSites());
        exception.Message.ShouldContain("Availability is required and must not be unknown.");
    }

    /// <summary>
    /// Tests that the <see cref="SiteListService.GetAllSites" /> method returns all sites when the notes are not provided.
    /// </summary>
    [Fact]
    public void GetAllSites_WithMissingNotes_ReturnsAllSites()
    {
        Dictionary<string, string?> siteConfiguration = new()
        {
            ["SiteList:Sites:0:Title"] = "Site 1",
            ["SiteList:Sites:0:Url"] = "https://example1.co.uk",
            ["SiteList:Sites:0:Description"] = "Site 1 Description",
            ["SiteList:Sites:0:Hosting"] = "Site 1 Hosting",
            ["SiteList:Sites:0:SourceCodeUrl"] = "https://source.example1.co.uk",
            ["SiteList:Sites:0:Availability"] = "AlwaysAvailable",
            ["SiteList:Sites:0:Notes"] = null
        };
        
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(siteConfiguration)
            .Build();
        
        SiteListService siteListService = new(configuration, new SiteValidator());
        
        List<Site> sites = siteListService.GetAllSites();
        
        sites.Count.ShouldBe(1);
    }
}