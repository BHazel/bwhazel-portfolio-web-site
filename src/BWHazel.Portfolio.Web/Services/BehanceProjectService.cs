using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace BWHazel.Portfolio.Web.Services;

/// <summary>
/// Works with Behance projects.
/// </summary>
/// <param name="configuration">The application configuration</param>
public class BehanceProjectService(IConfiguration configuration)
{
    private const string GraphicDesignBehanceProjectsWorksKey = "GraphicDesign:BehanceProjects:Works";

    private readonly IConfiguration configuration = configuration;

    /// <summary>
    /// Gets all Behance project IDs.
    /// </summary>
    /// <returns>A collection of all Behance project IDs.</returns>
    public List<int> GetAllBehanceProjectIds()
    {
        IConfigurationSection graphicDesignBehanceProjectsWorksSection =
            this.configuration.GetSection(GraphicDesignBehanceProjectsWorksKey);

        List<int> behanceProjectIds = graphicDesignBehanceProjectsWorksSection
            .GetChildren()
            .Select(section => int.TryParse(section["BehanceProjectId"]!, out int behanceProjectId)
                ? behanceProjectId
                : 0)
            .ToList();

        if (behanceProjectIds.Any(behanceProjectId => behanceProjectId == 0))
        {
            throw new ArgumentException("Behance project IDs are required and must be integers.");
        }

        return behanceProjectIds;
    }
}