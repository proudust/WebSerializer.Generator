using Microsoft.CodeAnalysis;
using System;

namespace Proudust.WebSerializer.Generator;

[Generator(LanguageNames.CSharp)]
public sealed partial class WebSerializerGenerator : IIncrementalGenerator
{
    public const string GenerateWebSerializerAttributeFullName = "Proudust.WebSerializer.Generator.GenerateWebSerializerAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static context =>
        {
            context.AddSource("GenerateWebSerializerAttribute.cs", /* lang=c#-test */ """
                using System;

                namespace Proudust.WebSerializer.Generator;

                [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
                internal sealed class GenerateWebSerializerAttribute : Attribute;
                """);
        });
    }
}
