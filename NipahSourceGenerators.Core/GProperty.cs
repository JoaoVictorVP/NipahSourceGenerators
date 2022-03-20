using NipahSourceGenerators.Core.CSharp;
using System.Text;

namespace NipahSourceGenerators.Core;

public class GProperty : GMember
{
    public GTypeRef PropertyType;
    public bool IsIndex;
    public List<GParameter> IndexParameters;

    public GMethodRef Getter, Setter;

    public GProperty WithGetter(GMethodRef getter)
    {
        Getter = getter;
        return this;
    }
    public GProperty WithSetter(GMethodRef setter)
    {
        Setter = setter;
        return this;
    }

    public GProperty AsIndex()
    {
        IsIndex = true;
        return this;
    }
    public GProperty WithIndexParameter(GParameter param)
    {
        IndexParameters.Add(param);
        return this;
    }

    public override void GenerateSourceCode(StringBuilder code)
    {
        
    }
}
