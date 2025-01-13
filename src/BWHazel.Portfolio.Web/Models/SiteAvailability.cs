using System.ComponentModel;

namespace BWHazel.Portfolio.Web.Models;

/// <summary>
/// Defines constants for the availability of a site.
/// </summary>
public enum SiteAvailability
{
    /// <summary>
    /// Fully public and available at all times.
    /// </summary>
    [Description("Always Available")]
    AlwaysAvailable,
    
    /// <summary>
    /// Not intended for general access but targeted at specific groups and may be limited to specific timeframes.
    /// </summary>
    [Description("Limited Access")]
    LimitedAccess,
    
    /// <summary>
    /// Taken offline and no longer available.
    /// </summary>
    [Description("Taken Offline")]
    TakenOffline,
    
    /// <summary>
    /// Availability is unknown.
    /// </summary>
    [Description("Unknown")]
    Unknown
}