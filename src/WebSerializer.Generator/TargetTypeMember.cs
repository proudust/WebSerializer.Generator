using System.Runtime.Serialization;
using Microsoft.CodeAnalysis;

namespace Proudust.Web;

public sealed class TargetTypeMember
{
    public int Order { get; }

    public string Type { get; }

    public bool IsNullable { get; }

    public string MemberName { get; }

    public TargetTypeMember(IPropertySymbol symbol, int order)
    {
        var attr = symbol.GetAttributes()
            .FirstOrDefault(x => x.AttributeClass?.Name is nameof(DataMemberAttribute));

        Order = attr?.NamedArguments
            .FirstOrDefault(x => x.Key is nameof(DataMemberAttribute.Order))
            .Value switch
        {
            { IsNull: true } => order,
            { IsNull: false, Value: object v } => (int)v,
            _ => throw new NotImplementedException(),
        };
        Type = symbol.Type.Name;
        IsNullable = !symbol.Type.IsValueType;
        MemberName = symbol.Name;
    }
}
