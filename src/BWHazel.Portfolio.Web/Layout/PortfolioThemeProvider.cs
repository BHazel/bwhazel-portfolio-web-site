using MudBlazor;
using MudBlazor.Utilities;

namespace BWHazel.Portfolio.Web.Layout;

/// <summary>
/// Provides the theme for the application.
/// </summary>
public static class PortfolioThemeProvider
{
    /// <summary>
    /// Gets the theme for the application.
    /// </summary>
    public static MudTheme PortfolioTheme => new()
    {
        PaletteDark = new()
        {
            Primary = Colors.Teal.Default,
            Secondary = Colors.DeepPurple.Default
        }
    };
    
    /// <summary>
    /// Gets the default primary colour for the application.
    /// </summary>
    public static MudColor PrimaryColour => PortfolioTheme.PaletteDark.Primary;
    
    /// <summary>
    /// Gets the lighter primary colour for the application.
    /// </summary>
    public static MudColor PrimaryLightenColour => PortfolioTheme.PaletteDark.PrimaryLighten;
    
    /// <summary>
    /// Gets the darker primary colour for the application.
    /// </summary>
    public static MudColor PrimaryDarkenColour => PortfolioTheme.PaletteDark.PrimaryDarken;
    
    /// <summary>
    /// Gets the default secondary colour for the application.
    /// </summary>
    public static MudColor SecondaryColour => PortfolioTheme.PaletteDark.Secondary;
    
    /// <summary>
    /// Gets the lighter secondary colour for the application.
    /// </summary>
    public static MudColor SecondaryLightenColour => PortfolioTheme.PaletteDark.SecondaryLighten;
    
    /// <summary>
    /// Gets the darker secondary colour for the application.
    /// </summary>
    public static MudColor SecondaryDarkenColour => PortfolioTheme.PaletteDark.SecondaryDarken;
}