using Microsoft.CodeAnalysis;

namespace Proudust.Web;

public static class RoslynExtensions
{
    public static IEnumerable<INamedTypeSymbol> EnumerateContainingTypes(this ISymbol symbol)
    {
        INamedTypeSymbol? containingType = symbol.ContainingType;
        while (containingType is not null)
        {
            yield return containingType;
            containingType = containingType.ContainingType;
        }
    }

    public static T? GetNamedArgument<T>(this AttributeData attribute, string key, T? defaultValue = default)
    {
        return attribute.NamedArguments.FirstOrDefault(x => x.Key == key).Value switch
        {
            { Value: T value } => value,
            _ => defaultValue,
        };
    }

    public static string GetTypeKeyword(this INamedTypeSymbol symbol)
    {
        return symbol switch
        {
            { IsRecord: true, IsValueType: true } => "record struct",
            { IsRecord: true, IsValueType: false } => "record",
            { IsRecord: false, IsValueType: true } => "struct",
            { IsRecord: false, IsValueType: false } => "class",
        };
    }
}
