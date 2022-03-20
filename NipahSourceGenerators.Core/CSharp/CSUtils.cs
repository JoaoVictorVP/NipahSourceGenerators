namespace NipahSourceGenerators.Core.CSharp;

public static class CSUtils
{
    public static string ToCSInline(this IEnumerable<GParameter> pars)
    {
        string final = "";
        foreach (var param in pars)
        {
            if (final != null)
                final += ", ";
            final += ((GParameterRef)param).ToCSInline();
        }
        return final;
    }
    public static string ToCSInline(this IEnumerable<GParameterRef> pars)
    {
        string final = "";
        foreach(var param in pars)
        {
            if (final != null)
                final += ", ";
            final += param.ToCSInline();
        }
        return final;
    }
    public static string ToCSInline(this GParameterRef param)
    {
        return $"{param.Type.FullName} {param.Name}{(param.DefaultValue != null ? " = " + param.DefaultValue : "")}";
    }

    public static string ToCS(this MemberVisibility visibility) => visibility switch
    {
        MemberVisibility.Public => "public ",
        MemberVisibility.Private => "private ",
        MemberVisibility.Protected => "protected ",
        MemberVisibility.Internal => "internal ",
        _ => ""
    };
    public static string ToCS(this MemberModifier modifier)
    {
        if (modifier == MemberModifier.None)
            return "";

        string final = "";

        if (modifier.HasFlag(MemberModifier.Unsafe))
            final = "unsafe ";

        if(modifier.HasFlag(MemberModifier.Static))
            final += "static ";

        if (modifier.HasFlag(MemberModifier.Const))
            final += "const ";

        if (modifier.HasFlag(MemberModifier.ReadOnly))
            final += "readonly ";

        if (modifier.HasFlag(MemberModifier.Partial))
            final += "partial ";

        if (modifier.HasFlag(MemberModifier.Virtual))
            final += "virtual ";

        if (modifier.HasFlag(MemberModifier.Abstract))
            final += "abstract ";

        return final;
    }
}