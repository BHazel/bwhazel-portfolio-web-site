using BWHazel.Portfolio.Web.Layout;
using MudBlazor;
using MudBlazor.Utilities;
using Shouldly;
using Xunit;

namespace BWHazel.Portfolio.Web.Test.Layout;

/// <summary>
/// Tests for the <see cref="PortfolioThemeProvider" /> class.
/// </summary>
public class PortfolioThemeProviderTests
{
    /// <summary>
    /// Tests that the <see cref="PortfolioThemeProvider.PortfolioTheme" /> property returns the correctly configured theme.
    /// </summary>
    [Fact]
    public void PortfolioTheme_ReturnsCorrectlyConfiguredTheme()
    {
        MudTheme portfolioTheme = PortfolioThemeProvider.PortfolioTheme;
        
        portfolioTheme.ShouldBeEquivalentTo(PortfolioTheme);
    }
    
    /// <summary>
    /// Tests that the <see cref="PortfolioThemeProvider.PrimaryColour" /> property returns the correctly configured primary colour.
    /// </summary>
    [Fact]
    public void PrimaryColour_ReturnsCorrectlyConfiguredColour()
    {
        MudColor primaryColour = PortfolioThemeProvider.PrimaryColour;
        
        primaryColour.ShouldBe(PortfolioTheme.PaletteDark.Primary);
    }
    
    /// <summary>
    /// Tests that the <see cref="PortfolioThemeProvider.PrimartLightenColour" /> property returns the correctly configured primary lighter colour.
    /// </summary>
    [Fact]
    public void PrimaryLightenColour_ReturnsCorrectlyConfiguredColour()
    {
        MudColor primaryLightenColour = PortfolioThemeProvider.PrimaryLightenColour;
        
        primaryLightenColour.ShouldBe(PortfolioTheme.PaletteDark.PrimaryLighten);
    }
    
    /// <summary>
    /// Tests that the <see cref="PortfolioThemeProvider.PrimaryDarkenColour" /> property returns the correctly configured primary darker colour.
    /// </summary>
    [Fact]
    public void PrimaryDarkenColour_ReturnsCorrectlyConfiguredColour()
    {
        MudColor primaryDarkenColour = PortfolioThemeProvider.PrimaryDarkenColour;
        
        primaryDarkenColour.ShouldBe(PortfolioTheme.PaletteDark.PrimaryDarken);
    }
    
    /// <summary>
    /// Tests that the <see cref="PortfolioThemeProvider.SecondaryColour" /> property returns the correctly configured secondary colour.
    /// </summary>
    [Fact]
    public void SeconraryColour_ReturnsCorrectlyConfiguredColour()
    {
        MudColor secondaryColour = PortfolioThemeProvider.SecondaryColour;
        
        secondaryColour.ShouldBe(PortfolioTheme.PaletteDark.Secondary);
    }
    
    /// <summary>
    /// Tests that the <see cref="PortfolioThemeProvider.SecondaryLightenColour" /> property returns the correctly configured secondary lighter colour.
    /// </summary>
    [Fact]
    public void SecondaryLightenColour_ReturnsCorrectlyConfiguredColour()
    {
        MudColor secondaryLightenColour = PortfolioThemeProvider.SecondaryLightenColour;
        
        secondaryLightenColour.ShouldBe(PortfolioTheme.PaletteDark.SecondaryLighten);
    }
    
    /// <summary>
    /// Tests that the <see cref="PortfolioThemeProvider.SecondaryDarkenColour" /> property returns the correctly configured secondary darker colour.
    /// </summary>
    [Fact]
    public void SecondaryDarkenColour_ReturnsCorrectlyConfiguredColour()
    {
        MudColor secondaryDarkenColour = PortfolioThemeProvider.SecondaryDarkenColour;
        
        secondaryDarkenColour.ShouldBe(PortfolioTheme.PaletteDark.SecondaryDarken);
    }
    
    /// <summary>
    /// Gets the portfolio theme.
    /// </summary>
    private static MudTheme PortfolioTheme => new()
    {
        PaletteDark = new()
        {
            Primary = Colors.Teal.Default,
            Secondary = Colors.DeepPurple.Default
        }
    };
}