using System;
using System.Text;

namespace NipahSourceGenerators.Core.CSharp;

public class CSClass : GType<CSField, CSProperty, CSMethod>
{
    public new CSClass WithName(string name) => base.WithName(name).As<CSClass>();

    public new CSClass WithVisibility(MemberVisibility visibility) => base.WithVisibility(visibility).As<CSClass>();

    public new CSClass WithModifiers(MemberModifier modifiers) => base.WithModifiers(modifiers).As<CSClass>();

    public new CSClass WithField(CSField field) => base.WithField(field).As<CSClass>();
    public new CSClass WithProperty(CSProperty prop) => base.WithProperty(prop).As<CSClass>();
    public new CSClass WithMethod(CSMethod method) => base.WithMethod(method).As<CSClass>();

    public override void GenerateSourceCode(StringBuilder code)
    {
        if(Modifiers.HasFlag(MemberModifier.Static))
        {
            if (Fields.Exists(f => !f.Modifiers.HasFlag(MemberModifier.Static)))
                throw new Exception("All fields in a static class should be static");
            if (Properties.Exists(f => !f.Modifiers.HasFlag(MemberModifier.Static)))
                throw new Exception("All properties in a static class should be static");
            if (Methods.Exists(f => !f.Modifiers.HasFlag(MemberModifier.Static)))
                throw new Exception("All methods in a static class should be static");
        }

        // Namespace
        if (!string.IsNullOrEmpty(Namespace))
            code.AppendLine($"namespace {Namespace};");

        // Header
        code.AppendLine($"{Visibility.ToCS()}{Modifiers.ToCS()}class {Name}");
        code.AppendLine("{");

        // Body
        Fields.ForEach(field => field.GenerateSourceCode(code));
        Properties.ForEach(prop => prop.GenerateSourceCode(code));
        Methods.ForEach(method => method.GenerateSourceCode(code));

        // Closure
        code.AppendLine("}");
    }
}
