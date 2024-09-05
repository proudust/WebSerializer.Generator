using System.Runtime.Serialization;
using Cysharp.Web;
using Microsoft.CodeAnalysis;

namespace Proudust.Web;

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

        Order = dataMemberAttr?.NamedArguments
            .FirstOrDefault(x => x.Key is nameof(DataMemberAttribute.Order))
            .Value switch
        {
            { Value: int v } => v,
            _ => int.MaxValue,
        };
        Type = symbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        IsNullable = symbol.Type is { IsValueType: false } or { Name: "Nullable" };
        MemberName = symbol.Name;
        SerializedName = dataMemberAttr?.NamedArguments
            .FirstOrDefault(x => x.Key is nameof(DataMemberAttribute.Name))
            .Value switch
        {
            { Value: string v } => v,
            _ => symbol.Name,
        };
        WebSerializer = webSerializerAttr?.ConstructorArguments[0] switch
        {
            { Value: INamedTypeSymbol webSerializerType } => webSerializerType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            _ => null,
        };
    }
}
