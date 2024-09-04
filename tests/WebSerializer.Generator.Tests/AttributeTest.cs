// Based On: https://github.com/Cysharp/WebSerializer/blob/1.3.0/tests/WebSerializer.Tests/AttributeTest.cs
using System.Runtime.Serialization;
using Cysharp.Web;
using Cysharp.Web.Providers;
using FluentAssertions;
using Proudust.Web;

namespace WebSerializerTests.Generator;

public class AttributeTest
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
        ]),
    };

    [Fact]
    public void Attr1()
    {
        var req = new MyRequest { One = 9, Two = "hoge", ACustomInt = 100, NoMember = MyEnum.Boyo };

        var q = WebSerializer.ToQueryString(req, options);

        q.Should().Be("tweet.One=9&tweet.twooooo=hoge&tweet.ACustomInt=10000&tweet.NoMember=tako");
    }
}

[DataContract(Namespace = "tweet.")]
[GenerateWebSerializer]
public partial class MyRequest
{
    [DataMember(Order = 0)]
    public int One { get; set; }

    [DataMember(Name = "twooooo", Order = 1)]
    public string? Two { get; set; }

    public MyEnum NoMember { get; set; }

    [WebSerializer(typeof(CustomFormatterForInt))]
    [DataMember(Order = 2)]
    public int ACustomInt { get; set; }
}

public enum MyEnum
{
    Foo,

    [EnumMember(Value = "tako")]
    Boyo
}

public class CustomFormatterForInt : IWebSerializer<int>
{
    public void Serialize(ref WebSerializerWriter writer, int value, WebSerializerOptions options)
    {
        writer.GetStringBuilder().Append(value * value);
    }
}
