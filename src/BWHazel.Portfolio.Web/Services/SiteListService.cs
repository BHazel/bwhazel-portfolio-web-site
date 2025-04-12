using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using BWHazel.Portfolio.Web.Models;
using FluentValidation;

namespace BWHazel.Portfolio.Web.Services;

/// <summary>
/// Works with sites on the Site List.
/// </summary>
/// <param name="configuration">The application configuration.</param>
public class SiteListService(IConfiguration configuration, IValidator<Site> siteValidator)
{
    private const string SiteListSitesKey = "SiteList:Sites";

    private readonly IConfiguration configuration = configuration;
    private readonly IValidator<Site> siteValidator = siteValidator;

    /// <summary>
    /// Gets all sites.
    /// </summary>
    /// <returns>A collection of all sites.</returns>
    public List<Site> GetAllSites()
    {
        IConfigurationSection siteListSitesSection = this.configuration.GetSection(SiteListSitesKey);
        List<Site> sites = siteListSitesSection
            .GetChildren()
            .Select(MapConfigurationToSite)
            .ToList();

        return sites;
    }

    /// <summary>
    /// Maps a configuration section to a site.
    /// </summary>
    /// <param name="siteListSiteSection"></param>
    /// <returns>A site.</returns>
    private Site MapConfigurationToSite(IConfigurationSection siteListSiteSection)
    {
        Site site = new(
            siteListSiteSection["Title"]!,
            siteListSiteSection["Url"],
            siteListSiteSection["Description"]!,
            siteListSiteSection["Hosting"]!,
            siteListSiteSection["SourceCodeUrl"],
            Enum.TryParse(siteListSiteSection["Availability"]!, out SiteAvailability availability)
                ? availability
                : SiteAvailability.Unknown,
            siteListSiteSection["Notes"]!
        );

        this.siteValidator.ValidateAndThrow(site);
        return site;
    }
}