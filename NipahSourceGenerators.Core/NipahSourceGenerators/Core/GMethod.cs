using NipahSourceGenerators.Core.Expressions;
using System.Collections.Generic;
using System.Text;

namespace NipahSourceGenerators.Core;

public class GMethod : GMember
{
    public List<GParameter> Parameters = new List<GParameter>(32);
    public Expression Body;
    public GTypeRef ReturnType;

    public GMethod WithParameter(GParameter param)
    {
        Parameters.Add(param);
        return this;
    }

    public GMethod WithBody(Expression body)
    {
        Body = body;
        return this;
    }

    public virtual void GenerateBodySourceCode(StringBuilder code)
    {

    }
}

public struct GParameter
{
    public string Name;
    public GTypeRef Type;
    public string DefaultValue;
}