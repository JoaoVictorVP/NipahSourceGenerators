global using defGType = NipahSourceGenerators.Core.GType<NipahSourceGenerators.Core.GField, NipahSourceGenerators.Core.GProperty, NipahSourceGenerators.Core.GMethod>;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace NipahSourceGenerators.Core;

public static class RefExtensions
{
    public static GTypeRef AsRef(this ITypeSymbol type) => new GTypeRef { s_type = type };
    public static GMethodRef AsRef(this IMethodSymbol method) => new GMethodRef { s_method = method };
    public static GParameterRef AsRef(this IParameterSymbol param) => new GParameterRef { s_param = param };
}

public struct GTypeRef
{
    defGType g_type;
    Type def_type;
    internal ITypeSymbol s_type;

    public bool IsNull => g_type == null && def_type == null && s_type == null;

    public string Name => g_type != null ? g_type.Name : (def_type != null ? def_type.Name : s_type.Name);
    public string FullName => g_type != null ? g_type.FullName : (def_type != null ? (def_type == typeof(void) ? "void" : def_type.FullName) : s_type.ToDisplayString());
    public GTypeRef BaseType => g_type != null ? g_type.BaseType : (def_type != null ? def_type.BaseType : s_type.BaseType.AsRef());

    public defGType TryGType() => g_type;
    public Type TryDefaultType() => def_type;

    public static implicit operator GTypeRef (defGType type) => new GTypeRef { g_type = type };
    public static implicit operator GTypeRef (Type type) => new GTypeRef { def_type = type };
    //public static implicit operator GTypeRef(ITypeSymbol type) => new GTypeRef { s_type = type };
}
public struct GMethodRef
{
    GMethod g_method;
    MethodInfo def_method;
    internal IMethodSymbol s_method;

    public bool IsNull => g_method == null && def_method == null && s_method == null;

    public string Name => g_method != null ? g_method.Name : (def_method != null ? def_method.Name : s_method.Name);
    public GTypeRef ReturnType => g_method != null ? g_method.ReturnType : (def_method != null ? def_method.ReturnType : s_method.ReturnType.AsRef());
    public IEnumerator<GParameterRef> Parameters
    {
        get
        {
            if(g_method != null)
            {
                foreach (var param in g_method.Parameters)
                    yield return param;
            }
            else if(def_method != null)
            {
                foreach (var param in def_method.GetParameters())
                    yield return param;
            }
            else
            {
                foreach(var param in s_method.Parameters)
                    yield return param.AsRef();
            }
        }
    }

    public static implicit operator GMethodRef(GMethod method) => new GMethodRef { g_method = method };
    public static implicit operator GMethodRef(MethodInfo method) => new GMethodRef { def_method = method };

    public void BuildBody(StringBuilder code)
    {
        if (g_method != null)
            g_method.GenerateBodySourceCode(code);
        else
            code.Append($"// MethodInfo should really generate something here?");
    }
}
public struct GParameterRef
{
    GParameter g_param;
    ParameterInfo def_param;
    internal IParameterSymbol s_param;

    public bool IsNull => g_param.Name == null && def_param == null && s_param == null;

    public string Name => g_param.Name != null ? g_param.Name : (def_param != null ? def_param.Name : s_param.Name);
    public GTypeRef Type => g_param.Name != null ? g_param.Type : (def_param != null ? def_param.ParameterType : s_param.Type.AsRef());
    public string DefaultValue => g_param.Name != null ? g_param.DefaultValue : (def_param != null ? def_param.RawDefaultValue.ToString() : s_param.ExplicitDefaultValue.ToString());

    public static implicit operator GParameterRef(GParameter parameter) => new GParameterRef { g_param = parameter };
    public static implicit operator GParameterRef(ParameterInfo parameter) => new GParameterRef { def_param = parameter };
}