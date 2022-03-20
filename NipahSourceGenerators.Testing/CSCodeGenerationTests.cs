using static NipahSourceGenerators.Core.Expressions.ExpressionUtils_Procedural;
using NipahSourceGenerators.Core.CSharp;
using NUnit.Framework;
using Microsoft.CSharp;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System;
using NipahSourceGenerators.Core;

namespace NipahSourceGenerators.Testing;

public class CSCodeGenerationTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void HelloWorldBuilder()
    {
        string code = new CodeBuilder()
            .Class("Program", modifiers: MemberModifier.Static)
                .Field("Message", typeof(string), "Hello, World!", modifiers: MemberModifier.Static)
                .Method("Main", typeof(void), modifiers: MemberModifier.Static)
                    .Invoke(typeof(Console), "WriteLine", Value.Source("Message"))
                .End()
            .End()
        .Build();

        CompileSource(code);
    }

    [Test]
    public void InvokeOnField()
    {
        string code = new CodeBuilder()
            .Class("Program")
                .Field("Message", typeof(string), new InvokeBuilder(new[] { "BuildMessage" }, "Hello, World!"))
                .Method("BuildMessage", typeof(string), new[] { new ParamBuilder("message", typeof(string), "Hello, World!") })
                    .Line("return message;")
                .End()
            .End()
        .Build();

        CompileSource(code);
    }

    [Test]
    public void EmptyClass()
    {
        string code = new CodeBuilder()
            .Class("Test")
            .End()
        .Build();

        CompileSource(code);
    }

    [Test]
    public void EmptyProperty()
    {
        string code = new CodeBuilder()
            .Class("Test")
                .Property("Message", typeof(string), defValue: default)
            .End()
        .Build();

        CompileSource(code);
    }
    [Test]
    public void Property()
    {
        string code = new CodeBuilder()
            .Class("Test")
                .Property("Message", typeof(string))
                    .Getter()
                        .Line("return \"Hello, World!\";")
                    .EndAcessor()
                    .Setter()
                        .Line("throw new System.Exception();")
                    .EndAcessor()
                .End()
            .End()
        .Build();

        CompileSource(code);
    }

    [Test]
    public void SimpleHelloWorldClassWithSource()
    {
        var csClass = new CSClass()
            .WithName("Program").As<CSClass>()
            .WithField(new CSField("Message", typeof(string)).WithDefaultValue("\"Hello, World\"").As<CSField>())
            .WithMethod(new CSMethod("Main", typeof(void))
                .WithBody(SourceExp("System.Console.WriteLine(Message);")).As<CSMethod>());

        string code = csClass.GenerateSourceCode();

        CompileSource(code);
    }
    [Test]
    public void SimplePartialClassWithPropertyGet()
    {
        var csClass = new CSClass()
            .WithName("TestClass")
            .WithModifiers(MemberModifier.Partial)
            .WithField(new CSField("_backingField", typeof(int))
                .WithVisibility(MemberVisibility.Private)
                .WithModifiers(MemberModifier.ReadOnly).As<CSField>())
            .WithProperty(
                new CSProperty("Property", typeof(int))
                    .WithGetter(new CSMethod()
                        .WithBody(SourceExp("return _backingField;"))).As<CSProperty>()
                     );

        string code = csClass.GenerateSourceCode();

        CompileSource(code);
    }
    [Test]
    public void SimplePartialStaticClassWithPropertyGetAndSet()
    {
        var csClass = new CSClass()
            .WithName("TestClass")
            .WithModifiers(Core.MemberModifier.Partial|Core.MemberModifier.Static).As<CSClass>()
            .WithField(new CSField("_backingField", typeof(int))
                .WithVisibility(Core.MemberVisibility.Private)
                .WithModifiers(Core.MemberModifier.ReadOnly|Core.MemberModifier.Static).As<CSField>())
            .WithProperty(
                new CSProperty("Property", typeof(int))
                    .WithGetter(new CSMethod()
                        .WithBody(SourceExp("return _backingField;")))
                    .WithSetter(new CSMethod()
                        .WithBody(SourceExp("_backingField = value;")))
                        .WithModifiers(Core.MemberModifier.Static)
                        .As<CSProperty>()
                     );

        string code = csClass.GenerateSourceCode();

        CompileSource(code);
    }

    void CompileSource(string code)
    {
        Console.WriteLine(code);

        var tree = CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(LanguageVersion.Preview, Microsoft.CodeAnalysis.DocumentationMode.None, Microsoft.CodeAnalysis.SourceCodeKind.Regular));

        var diagnostic = tree.GetDiagnostics();
        diagnostic = diagnostic.Where(d => d.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error);
        Assert.IsEmpty(diagnostic, "Generated C# Code should'nt contain any errors");
    }
}
