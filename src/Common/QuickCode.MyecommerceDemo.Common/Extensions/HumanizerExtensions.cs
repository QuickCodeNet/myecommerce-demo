using Humanizer;

namespace QuickCode.MyecommerceDemo.Common.Extensions;

public static class HumanizerExtensions
{
    public static string PluralizeForce(this string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return word;

        var allUpper = word.All(char.IsUpper) || word.Contains('_');
        var plural = word.Pluralize();

        return allUpper ? plural.ToUpperInvariant() : plural;
    }
}