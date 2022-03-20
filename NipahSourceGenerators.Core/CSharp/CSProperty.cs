using System.Text;

namespace NipahSourceGenerators.Core.CSharp;

public class CSProperty : GProperty
{
    public override void GenerateSourceCode(StringBuilder code)
    {
        code.Append($"{Visibility.ToCS()}{Modifiers.ToCS()}{PropertyType.FullName} ");
        if (IsIndex)
            code.Append($"this[{IndexParameters.ToCSInline()}]");
        else
            code.Append($"{Name}");

        int not = 0;

        if (Getter.IsNull) not++;
        if (Setter.IsNull) not++;

        if (not < 2)
        {
            code.AppendLine("\n{");
        }

        if (!Getter.IsNull)
        {
            code.AppendLine("get \n{");
            Getter.BuildBody(code);
            code.AppendLine("}");
        }
        if (!Setter.IsNull)
        {
            code.AppendLine("set \n{");
            Setter.BuildBody(code);
            code.AppendLine("}");
        }

        if (not > 1)
            code.AppendLine(";");
        else
            code.AppendLine("\n}");
    }

    public CSProperty(string name, GTypeRef propertyType)
    {
        Name = name;
        PropertyType = propertyType;
    }
}