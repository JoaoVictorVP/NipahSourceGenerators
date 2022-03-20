using System.Text;

namespace NipahSourceGenerators.Core.Expressions;

public class ComparisonExpression : BinaryExpression
{
    public ExpComparison Term;

    public override void CSGenerateSourceCode(StringBuilder code)
    {
        code.Append('(');

        Left.CSGenerateSourceCode(code);
        code.Append(Term switch
        {
            ExpComparison.Equal => " == ",
            ExpComparison.Different => " != ",
            ExpComparison.Larger => " > ",
            ExpComparison.Lower => " < ",
            ExpComparison.LargerOrEqual => " >= ",
            ExpComparison.LowerOrEqual => " <= ",
            _ => ""
        });
        Right.CSGenerateSourceCode(code);

        code.Append(')');
    }
}
public enum ExpComparison
{
    Equal,
    Different,
    Larger,
    Lower,
    LargerOrEqual,
    LowerOrEqual
}
public class AndExpression : UnaryExpression
{
    public override void CSGenerateSourceCode(StringBuilder code)
    {
        code.Append(" && ");
        Unary.CSGenerateSourceCode(code);
    }
}
public class OrExpression : UnaryExpression
{
    public override void CSGenerateSourceCode(StringBuilder code)
    {
        code.Append(" || ");
        Unary.CSGenerateSourceCode(code);
    }
}