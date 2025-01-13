using BWHazel.Portfolio.Web.Models;
using Shouldly;
using Xunit;

namespace BWHazel.Portfolio.Web.Test.Models;

/// <summary>
/// Tests for the <see cref="EnumExtensions" /> class.
/// </summary>
public class EnumExtensionsTests
{
    /// <summary>
    /// Tests that the <see cref="EnumExtensions.GetDescription" /> method returns the description for an enumeration value if present.
    /// </summary>
    [Fact]
    public void GetDescription_WithEnumValueWithDescription_ReturnsDescription()
    {
        TestEnum testEnum = TestEnum.HasDescription;

        string? description = testEnum.GetDescription();
        
        description.ShouldBe("Has Description");
    }
    
    /// <summary>
    /// Tests that the <see cref="EnumExtensions.GetDescription" /> method returns the string representation of the enumeration value if no description is present.
    /// </summary>
    [Fact]
    public void GetDescription_WithEnumValueWithoutDescription_ReturnsEnumValueName()
    {
        TestEnum testEnum = TestEnum.None;

        string? description = testEnum.GetDescription();
        
        description.ShouldBe("None");
    }
}