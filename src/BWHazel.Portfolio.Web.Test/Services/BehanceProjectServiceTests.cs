using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using BWHazel.Portfolio.Web.Services;
using Shouldly;
using Xunit;

namespace BWHazel.Portfolio.Web.Test.Services;

/// <summary>
/// Tests for the <see cref="BehanceProjectService"/> class.
/// </summary>
public class BehanceProjectServiceTests
{
    /// <summary>
    /// Tests that the <see cref="BehanceProjectService.GetAllBehanceProjectIds"/> method returns all Behance project IDs when present in configuration.
    /// </summary>
    [Fact]
    public void GetAllBehanceProjectIds_WithExistingProjects_ReturnsAllBehanceProjectIds()
    {
        Dictionary<string, string?> behanceProjectConfiguration = new()
        {
            ["GraphicDesign:BehanceProjects:Works:0:ProjectId"] = "project-1",
            ["GraphicDesign:BehanceProjects:Works:0:BehanceProjectId"] = "123",
            ["GraphicDesign:BehanceProjects:Works:1:ProjectId"] = "project-2",
            ["GraphicDesign:BehanceProjects:Works:1:BehanceProjectId"] = "456"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(behanceProjectConfiguration)
            .Build();

        BehanceProjectService behanceProjectService = new(configuration);

        List<int> behanceProjectIds = behanceProjectService.GetAllBehanceProjectIds();

        behanceProjectIds.Count.ShouldBe(2);
        behanceProjectIds.ShouldContain(123);
        behanceProjectIds.ShouldContain(456);
    }


    /// <summary>
    /// Tests that the <see cref="BehanceProjectService.GetAllBehanceProjectIds"/> method returns an empty list when no Behance projects are present in configuration.
    /// </summary>
    [Fact]
    public void GetAllBehanceProjectIds_WithNoExistingProjects_ReturnsEmptyList()
    {
        Dictionary<string, string?> behanceProjectConfiguration = [];
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(behanceProjectConfiguration)
            .Build();

        BehanceProjectService behanceProjectService = new(configuration);

        List<int> behanceProjectIds = behanceProjectService.GetAllBehanceProjectIds();

        behanceProjectIds.ShouldNotBeNull();
        behanceProjectIds.Count.ShouldBe(0);
    }

    /// <summary>
    /// Tests that the <see cref="BehanceProjectService.GetAllBehanceProjectIds"/> method throws an exception when the Behance project ID is invalid.
    /// </summary>
    /// <param name="invalidBehanceProjectId">The invalid Behance project ID.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("abc")]
    [InlineData("0")]
    public void GetAllBehanceProjectIds_WithInvalidBehanceProjectId_ThrowsException(string invalidBehanceProjectId)
    {
        Dictionary<string, string?> behanceProjectConfiguration = new()
        {
            ["GraphicDesign:BehanceProjects:Works:0:BehanceProjectId"] = invalidBehanceProjectId
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(behanceProjectConfiguration)
            .Build();

        BehanceProjectService behanceProjectService = new(configuration);

        ArgumentException exception = Should.Throw<ArgumentException>(() => behanceProjectService.GetAllBehanceProjectIds());
        exception.Message.ShouldBe("Behance project IDs are required and must be integers.");
    }

    /// <summary>
    /// Tests that the <see cref="BehanceProjectService.GetAllBehanceProjectIds"/> method throws an exception when the Behance project ID is missing.
    /// </summary>
    [Fact]
    public void GetAllBehanceProjectIds_WithMissingBehanceProjectId_ThrowsException()
    {
        Dictionary<string, string?> behanceProjectConfiguration = new()
        {
            ["GraphicDesign:BehanceProjects:Works:0:ProjectId"] = "project-1"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(behanceProjectConfiguration)
            .Build();

        BehanceProjectService behanceProjectService = new(configuration);

        ArgumentException exception = Should.Throw<ArgumentException>(() => behanceProjectService.GetAllBehanceProjectIds());
        exception.Message.ShouldBe("Behance project IDs are required and must be integers.");
    }
}