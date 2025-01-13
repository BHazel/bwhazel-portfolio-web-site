using System;
using FluentValidation;

namespace BWHazel.Portfolio.Web.Models.Validators;

/// <summary>
/// Defines extension methods for data validation.
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// Validates a string value is a valid image path.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="prefix">The path prefix, which is a suffix to the root 'img/' path.</param>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <returns>The rule builder.</returns>
    public static IRuleBuilderOptions<T, string> MustBeImagePath<T>(this IRuleBuilderOptions<T, string> ruleBuilder, string prefix = "")
    {
        return ruleBuilder.Must(str => str.StartsWith($"img/{prefix}"));
    }

    /// <summary>
    /// Validates a string value is in kebab case.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <returns>The rule builder.</returns>
    public static IRuleBuilderOptions<T, string> MustBeKebabCase<T>(this IRuleBuilderOptions<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(str => str.Length > 0 && !str.Contains(' '));
    }
    
    /// <summary>
    /// Validates a string value is a valid URI.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="nullable">Indicates if the value can be null or empty.</param>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <returns>The rule builder.</returns>
    public static IRuleBuilderOptions<T, string?> Uri<T>(this IRuleBuilder<T, string?> ruleBuilder, bool nullable)
    {
        return ruleBuilder.Must(str =>
        {
            try
            {
                if (nullable && string.IsNullOrEmpty(str))
                {
                    return true;
                }
                
                Uri uri = new(str!);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        });
    }
}