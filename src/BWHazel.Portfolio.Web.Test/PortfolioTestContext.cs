using Bunit;
using MudBlazor.Services;

namespace BWHazel.Portfolio.Web.Test;

/// <summary>
/// Enables testing of Blazor components.
/// </summary>
public class PortfolioTestContext : TestContext
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PortfolioTestContext"/> class.
    /// </summary>
    public PortfolioTestContext()
    {
        this.Services.AddMudServices();
    }
}