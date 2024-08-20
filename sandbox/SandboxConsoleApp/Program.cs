// See https://aka.ms/new-console-template for more information
using Cysharp.Web;
using Proudust.WebSerializer.Generator;
using System.Runtime.Serialization;

Console.WriteLine(WebSerializer.ToQueryString("https://www.google.co.jp/search", new UrlParams
{
    Query = "hello",
}));

[GenerateWebSerializer]
readonly partial struct UrlParams
{
    [DataMember(Name = "q")]
    public string Query { get; init; }
}
