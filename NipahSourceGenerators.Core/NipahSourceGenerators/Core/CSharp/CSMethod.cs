using System.Text;

namespace NipahSourceGenerators.Core.CSharp;

public class CSMethod : GMethod
{
    public override void GenerateSourceCode(StringBuilder code)
    {
        code.AppendLine($"{Visibility.ToCS()}{Modifiers.ToCS()}{Name}({Parameters.ToCSInline()})");
        code.AppendLine("{");
        GenerateBodySourceCode(code);
        code.AppendLine("}");
    }

    public override void GenerateBodySourceCode(StringBuilder code)
    {
        Body.CSGenerateSourceCode(code);
    }

    public CSMethod() { }

    public CSMethod(string name, GTypeRef returnType)
    {
        Name = name;
        ReturnType = returnType;
    }
}