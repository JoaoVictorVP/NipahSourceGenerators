global using static NipahSourceGenerators.Core.Expressions.ExpressionUtils_Procedural;
namespace NipahSourceGenerators.Core.Expressions;

public static class ExpressionUtils
{

}
public static class ExpressionUtils_Procedural
{
    public static IfExpression IfExp(ComparisonExpression comparison!!)
    {
        return new IfExpression { Comparison = comparison };
    }
    public static ComparisonExpression ComparisonExp(Expression left!!, ExpComparison term, Expression right!!)
    {
        return new ComparisonExpression { Left = left, Term = term, Right = right };
    }
    public static ElseExpression ElseExp()
    {
        return new ElseExpression();
    }

    public static SourceExpression SourceExp(string source!!)
    {
        return new SourceExpression { Source = source };
    }
}