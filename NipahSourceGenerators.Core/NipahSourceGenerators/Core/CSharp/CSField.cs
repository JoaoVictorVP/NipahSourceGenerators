using System.Text;

namespace NipahSourceGenerators.Core.CSharp;

public class CSField : GField
{
    public override void GenerateSourceCode(StringBuilder code)
    {
        code.AppendLine($"{Visibility.ToCS()}{Modifiers.ToCS()} {FieldType.FullName} {Name};");
    }

    public CSField(string name, GTypeRef fieldType)
    {
        Name = name;
        FieldType = fieldType;
    }
}
