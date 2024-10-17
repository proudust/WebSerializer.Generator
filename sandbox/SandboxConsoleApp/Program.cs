// See https://aka.ms/new-console-template for more information
using System.Runtime.Serialization;
using Cysharp.Web;
using Proudust.Web;

Console.WriteLine(WebSerializer.ToQueryString("https://www.google.co.jp/search", new UrlParams
{
    Query = "hello",
}));

readonly partial struct UrlParams
{
    [DataMember(Name = "q")]
    public string Query { get; init; }
}

[WebSerializable<UrlParams>]
sealed partial class GenerateWebSerializerProvider;
