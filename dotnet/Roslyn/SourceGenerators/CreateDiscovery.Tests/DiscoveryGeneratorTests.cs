using CreateDiscovery.Abstractions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreateDiscovery.Tests;

public class DiscoveryGeneratorTests
{
    [Fact]
    public void GenerateServicesWithDependencies()
    {
        CreateDiscoveryGenerator generator = new();

        CSharpGeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        CSharpCompilation compilation = CSharpCompilation.Create(
            nameof(DiscoveryGeneratorTests),
            [
                CSharpSyntaxTree.ParseText(AnalyzeCode.METHOD_SOURCE),
                CSharpSyntaxTree.ParseText(AnalyzeCode.SERVICE_CLASS_SOURCE),
            ],
            [
                // To support 'System.Attribute' inheritance, add reference to 'System.Private.CoreLib'.
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(DiscoverAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IServiceCollection).Assembly.Location),
            ]
        );

        GeneratorDriverRunResult runResult = driver.RunGenerators(compilation).GetRunResult();

        SyntaxTree generatedFileSyntax = runResult.GeneratedTrees.Single(t =>
            t.FilePath.EndsWith("archivo.g.cs")
        );

        string generatedText = generatedFileSyntax.ToString();
        Assert.Equal(
            AnalyzeCode.EXPECTED_GENERATED_TEXT,
            generatedText,
            ignoreCase: true,
            ignoreLineEndingDifferences: true,
            ignoreWhiteSpaceDifferences: true
        );
    }
}
