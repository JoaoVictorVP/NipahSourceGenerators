using Microsoft.CodeAnalysis;

namespace NipahSourceGenerators.Core;

/// <summary>
/// Base class wich all Nipah Source Generators should inherit
/// </summary>
public abstract class NipahSourceGenerator : IIncrementalGenerator
{
    public abstract void Initialize(IncrementalGeneratorInitializationContext context);
}
