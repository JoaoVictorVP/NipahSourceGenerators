using System.Text;

namespace NipahSourceGenerators.Core.Expressions;

public class SourceExpression : Expression
{
    public string Source;

    public override void CSGenerateSourceCode(StringBuilder code)
    {
        code.Append(Source);
    }
}