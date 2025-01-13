using System;
using System.ComponentModel;
using System.Reflection;

namespace BWHazel.Portfolio.Web.Models;

/// <summary>
/// Defines extension methods for enumerations.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the description for an enumeration value.
    /// </summary>
    /// <param name="enumValue">The enumeration value.</param>
    /// <returns>The description, otherwise the string representation of the value if not present.</returns>
    public static string? GetDescription(this Enum enumValue)
    {
        FieldInfo? enumField = enumValue.GetType().GetField(enumValue.ToString());
        if (enumField == null)
        {
            return null;
        }
        
        DescriptionAttribute? descriptionAttribute = enumField.GetCustomAttribute<DescriptionAttribute>();
        return descriptionAttribute is not null
            ? descriptionAttribute.Description
            : enumValue.ToString();
    }
}