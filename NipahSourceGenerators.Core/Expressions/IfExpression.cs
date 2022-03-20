using System.Text;

namespace NipahSourceGenerators.Core.Expressions;

public class IfExpression : UnaryExpression
{
    public ComparisonExpression Comparison;

    public override void CSGenerateSourceCode(StringBuilder code)
    {
        code.Append("if(");
        Comparison.CSGenerateSourceCode(code);
        code.AppendLine(")");
        new BlockExpression(Unary).CSGenerateSourceCode(code);
    }
}
public class ElseExpression : UnaryExpression
{
    public override void CSGenerateSourceCode(StringBuilder code)
    {
        code.AppendLine("else");
        new BlockExpression(Unary).CSGenerateSourceCode(code);
    }
}
