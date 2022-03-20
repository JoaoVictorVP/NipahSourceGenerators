using System.Collections.Generic;
using System.Text;

namespace NipahSourceGenerators.Core;

public class GMember
{
    public string Name;
    public string Comment;

    public MemberVisibility Visibility;
    public MemberModifier Modifiers;

    public GMember WithVisibility(MemberVisibility visibility)
    {
        Visibility = visibility;
        return this;
    }
    public GMember WithModifiers(MemberModifier modifiers)
    {
        Modifiers = modifiers;
        return this;
    }

    public List<GAttribute> Attributes = new List<GAttribute>(32);

    public TMember As<TMember>() where TMember : GMember => (TMember)this;

    public GMember WithName(string name)
    {
        Name = name;
        return this;
    }
    public GMember WithComment(string comment)
    {
        Comment = comment;
        return this;
    }

    public virtual void GenerateSourceCode(StringBuilder code)
    {

    }

    public string GenerateSourceCode()
    {
        StringBuilder code = new StringBuilder();
        GenerateSourceCode(code);
        return code.ToString();
    }
}
