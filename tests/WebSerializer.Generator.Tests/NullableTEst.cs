// Based On: https://github.com/Cysharp/WebSerializer/blob/1.3.0/tests/WebSerializer.Tests/NullableTEst.cs
using Cysharp.Web;
using Cysharp.Web.Providers;
using FluentAssertions;
using Proudust.Web;

namespace WebSerializerTests.Generator;

public partial class NullableTest
{
    private static readonly WebSerializerOptions options = WebSerializerOptions.Default with
    {
        Provider = WebSerializerProvider.Create([
            PrimitiveWebSerializerProvider.Instance,
            BuiltinWebSerializerProvider.Instance,
            AttributeWebSerializerProvider.Instance,
            GenericsWebSerializerProvider.Instance,
            CollectionWebSerializerProvider.Instance,
            ObjectFallbackWebSerializerProvider.Instance,
            // ObjectGraphWebSerializerProvider.Instance,
            GenerateWebSerializerProvider.Instance,
        ]),
    };

    [Fact]
    public void Obj()
    {
        var a = new NullableRequest()
        {
            Afoo = 100,
            Bbar = null,
            Ctor = 999
        };

        WebSerializer.ToQueryString(a, options).Should().Be("Afoo=100&Ctor=999");
    }

    [Fact]
    public void Dict()
    {
        var a = new Dictionary<string, int?>()
        {
            {"Afoo", 100 },
            {"Bbar", null },
            {"Ctor", 999 },
        };

        WebSerializer.ToQueryString(a, options).Should().Be("Afoo=100&Ctor=999");
    }

    [WebSerializable<NullableRequest>]
    sealed partial class GenerateWebSerializerProvider;
}

public partial class NullableRequest
{
    public int? Afoo { get; set; }
    public int? Bbar { get; set; }
    public int? Ctor { get; set; }
}
