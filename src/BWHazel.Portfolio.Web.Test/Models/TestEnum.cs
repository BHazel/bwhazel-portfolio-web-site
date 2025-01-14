using System.ComponentModel;
using BWHazel.Portfolio.Web.Models;

namespace BWHazel.Portfolio.Web.Test.Models;

/// <summary>
/// A test enumeration for testing the <see cref="EnumExtensions" /> class.
/// </summary>
public enum TestEnum
{
    /// <summary>
    /// Enumeration field with a description.
    /// </summary>
    [Description("Has Description")]
    HasDescription,

    /// <summary>
    /// Enumeration field without a description.
    /// </summary>
    None
}