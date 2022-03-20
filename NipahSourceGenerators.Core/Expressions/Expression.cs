using System.Text;

namespace NipahSourceGenerators.Core.Expressions;

public class Expression
{
    public virtual void CSGenerateSourceCode(StringBuilder code)
    {

    }
    public virtual void VBGenerateSourceCode(StringBuilder code)
    {

    }
    public virtual void FSGenerateSourceCode(StringBuilder code)
    {

    }
}
public class UnaryExpression : Expression
{
    public Expression Unary;

    public UnaryExpression Embed(Expression unary)
    {
        Unary = unary;
        return this;
    }
}
public class BinaryExpression : Expression
{
    public Expression Left;
    public Expression Right;

    public BinaryExpression WithLeft(Expression left)
    {
        Left = left;
        return this;
    }
    public BinaryExpression WithRight(Expression right)
    {
        Right = right;
        return this;
    }
}
public class BlockExpression : UnaryExpression
{
    public override void CSGenerateSourceCode(StringBuilder code)
    {
        code.AppendLine("{");

        Unary.CSGenerateSourceCode(code);

        code.AppendLine("}");
    }

    public BlockExpression(Expression contents)
    {
        Unary = contents;
    }
}
public class ParenthesisExpression : Expression
{

}
