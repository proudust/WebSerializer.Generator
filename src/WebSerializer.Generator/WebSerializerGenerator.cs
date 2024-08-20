using Microsoft.CodeAnalysis;
using System;

namespace Proudust.WebSerializer.Generator;

[Generator(LanguageNames.CSharp)]
public sealed partial class WebSerializerGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {

    }
}
