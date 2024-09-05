using System.Runtime.Serialization;
using Cysharp.Web;
using Microsoft.CodeAnalysis;

namespace Proudust.Web;

public sealed class TargetType
{
    public string? Namespace { get; }

    public (string TypeKeyword, string Name)[] Parents { get; }

    public string? Prefix { get; }

    public string TypeKeyword { get; }

    public string Name { get; }

    public string FullyQualifiedName { get; }

    public TargetTypeMember[] Members { get; }

    public TargetType(INamedTypeSymbol symbol)
    {
        Namespace = symbol.ContainingNamespace switch
        {
            { IsGlobalNamespace: false } ns => $"{ns}",
            _ => null,
        };
        Parents = symbol.EnumerateContainingTypes()
            .Select(symbol =>
            {
                string typeKeyword = symbol.GetTypeKeyword();
                string name = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                return (typeKeyword, name);
            })
            .Reverse()
            .ToArray();
        Prefix = symbol.GetAttributes()
            .FirstOrDefault(x => x.AttributeClass?.Name is nameof(DataContractAttribute))
            ?.GetNamedArgument<string>(nameof(DataContractAttribute.Namespace));
        TypeKeyword = symbol.GetTypeKeyword();
        Name = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        FullyQualifiedName = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        Members = symbol
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Select(symbol => new TargetTypeMember(symbol))
            .OrderBy(member => member.Order)
            .ToArray();
    }
}

public sealed class TargetTypeMember
{
    public int Order { get; }

    public string Type { get; }

    public bool IsNullable { get; }

    public string MemberName { get; }

    public string SerializedName { get; }

    public string? WebSerializer { get; }

    public TargetTypeMember(IPropertySymbol symbol)
    {
        var attrs = symbol.GetAttributes();
        var dataMemberAttr = attrs.FirstOrDefault(x => x.AttributeClass?.Name is nameof(DataMemberAttribute));
        var webSerializerAttr = attrs.FirstOrDefault(x => x.AttributeClass?.Name is nameof(WebSerializerAttribute));

        Order = dataMemberAttr?.GetNamedArgument(nameof(DataMemberAttribute.Order), int.MaxValue) ?? int.MaxValue;
        Type = symbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        IsNullable = symbol.Type is { IsValueType: false } or { Name: "Nullable" };
        MemberName = symbol.Name;
        SerializedName = dataMemberAttr?.GetNamedArgument<string>(nameof(DataMemberAttribute.Name)) ?? symbol.Name;
        WebSerializer = webSerializerAttr?.ConstructorArguments[0] switch
        {
            { Value: INamedTypeSymbol webSerializerType } => webSerializerType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            _ => null,
        };
    }
}
