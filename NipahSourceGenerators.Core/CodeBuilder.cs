using NipahSourceGenerators.Core.CSharp;
using System.Text;

namespace NipahSourceGenerators.Core;

/// <summary>
/// Less complex version of default type construction using <see cref="GType{TField, TProperty, TMethod}"/> and so on...
/// </summary>
public struct CodeBuilder
{
    StringBuilder code;

    public string Build() => code.ToString();

    public CodeBuilder Using(string @namespace)
    {
        code.AppendLine($"using {@namespace};");
        return this;
    }

    public CodeBuilder Namespace(string @namespace)
    {
        code.AppendLine($"namespace {@namespace};");
        return this;
    }

    public TypeBuilder Class(string name, MemberVisibility visibily = MemberVisibility.Public, MemberModifier modifiers = MemberModifier.None, GTypeRef baseType = default) => type(name, "class", visibily, modifiers, baseType);
    public TypeBuilder Struct(string name, MemberVisibility visibily = MemberVisibility.Public, MemberModifier modifiers = MemberModifier.None, GTypeRef baseType = default) => type(name, "struct", visibily, modifiers, baseType);

    TypeBuilder type(string name, string kind, MemberVisibility visibily = MemberVisibility.Public, MemberModifier modifiers = MemberModifier.None, GTypeRef baseType = default)
    {
        switch (modifiers)
        {
            case MemberModifier.Const: throw new Exception("Types cannot be constant");
            case MemberModifier.ReadOnly: throw new Exception("Types cannot be readonly");
            case MemberModifier.Virtual: throw new Exception("Types cannot be virtual");
        }

        code.Append($"{visibily.ToCS()}{modifiers.ToCS()}{kind} {name}");

        if (baseType.IsNull is false)
            code.AppendLine($" : {baseType.FullName} {{");
        else
            code.AppendLine(" {");

        return new TypeBuilder(code);
    }

    public CodeBuilder() => code = new StringBuilder(320);
    internal CodeBuilder(StringBuilder code) => this.code = code;
}
/// <summary>
/// Type, like: <code>public class TYPE : * { ... }</code>
/// </summary>
public struct TypeBuilder
{
    StringBuilder code;

    public TypeBuilder Field(string name, GTypeRef type, Value defValue = default, MemberVisibility visibility = MemberVisibility.Public, MemberModifier modifiers = MemberModifier.None)
    {
        code.Append($"{visibility.ToCS()}{modifiers.ToCS()}{type.FullName} {name}");
        if (defValue.IsNull is false)
            code.Append(" = " + defValue);
        code.AppendLine(";");

        return this;
    }

    public TypeBuilder Property(string name, GTypeRef type, Value defValue, MemberVisibility visibility = MemberVisibility.Public, MemberModifier modifier = MemberModifier.None)
    {
        code.Append($"{visibility.ToCS()}{modifier.ToCS()} {type.FullName} {name} {{ get; set; }}");
        if (defValue.IsNull is false)
            code.AppendLine($" = {defValue};");
        else
            code.AppendLine();
        return this;
    }

    public PropertyBuilder Property(string name, GTypeRef type, MemberVisibility visibility = MemberVisibility.Public, MemberModifier modifier = MemberModifier.None)
    {
        code.AppendLine($"{visibility.ToCS()}{modifier.ToCS()}{type.FullName} {name} {{");
        return new PropertyBuilder(code);
    }

    public MethodBuilder Method(string name, GTypeRef returnType, Span<ParamBuilder> parameters = default, MemberVisibility visibility = MemberVisibility.Public, MemberModifier modifiers = MemberModifier.None)
    {
        code.Append($"{visibility.ToCS()}{modifiers.ToCS()}{returnType.FullName} {name}(");

        bool first = true;
        foreach (var param in parameters)
        {
            if (!first)
                code.Append(',');
            else
                first = false;
            code.Append(param.ToString());
        }

        code.AppendLine(") {");

        return new MethodBuilder(code);
    }

    public CodeBuilder End()
    {
        code.AppendLine("}");
        return new CodeBuilder(code);
    }

    public TypeBuilder(StringBuilder code) => this.code = code;
}
/// <summary>
/// Property, like: <code>int X { get; set; }</code>
/// </summary>
public struct PropertyBuilder
{
    StringBuilder code;

    public PropertyBuilder EmptyGetter()
    {
        code.AppendLine("get;");
        return this;
    }
    public PropertyBuilder EmptySetter()
    {
        code.AppendLine("set;");
        return this;
    }

    public MethodBuilder Getter()
    {
        code.AppendLine("get {");
        return new MethodBuilder(code);
    }
    public MethodBuilder Setter()
    {
        code.AppendLine("set {");
        return new MethodBuilder(code);
    }
    public MethodBuilder Init()
    {
        code.AppendLine("init {");
        return new MethodBuilder(code);
    }

    public TypeBuilder End()
    {
        code.AppendLine("}");
        return new TypeBuilder(code);
    }

    public PropertyBuilder(StringBuilder code) => this.code = code;
}
/// <summary>
/// Method, like: <code>void METHOD(...) {...}</code>
/// </summary>
public struct MethodBuilder
{
    StringBuilder code;

    public MethodBuilder Line(string line)
    {
        code.AppendLine(line);
        return this;
    }

    public MethodBuilder Invoke(GTypeRef type, string methodName, params Value[] args)
    {
        code.AppendLine(new InvokeBuilder(type, methodName, args).ToString() + ";");
        return this;
    }

    public TypeBuilder End()
    {
        code.AppendLine("}");
        return new TypeBuilder(code);
    }
    public PropertyBuilder EndAcessor()
    {
        code.Append('}');
        return new PropertyBuilder(code);
    }

    public MethodBuilder(StringBuilder code) => this.code = code;
}
/// <summary>
/// Invoke, like <code>METHOD(x, ...);</code>
/// </summary>
public ref struct InvokeBuilder
{
    readonly Span<string> parts;
    readonly Span<Value> args;

    public override string ToString()
    {
        string invoke = "";

        bool first = true;
        foreach(var part in parts)
        {
            if (!first)
                invoke += '.';
            else
                first = false;

            invoke += part;
        }
        invoke += "(";

        first = true;
        foreach(var arg in args)
        {
            if (!first)
                invoke += ',';
            else
                first = false;

            invoke += arg;
        }
        invoke += ")";

        return invoke;
    }

    public InvokeBuilder(Span<string> parts, Span<Value> args)
    {
        this.parts = parts;
        this.args = args;
    }

    public InvokeBuilder(Span<string> parts, params Value[] args)
    {
        this.parts = parts;
        this.args = args;
    }
    /*public InvokeBuilder(Span<string> acessor, string methodName, params Value[] args)
    {
        parts = new string[acessor.Length + 1];
        acessor.CopyTo(parts);
        parts[parts.Length - 1] = methodName;

        this.args = args;
    }*/
    public InvokeBuilder(GTypeRef type, string methodName, params Value[] args)
    {
        parts = new[] { type.FullName, methodName };
        this.args = args;
    }
}
/// <summary>
/// Parameter, like: <code>(int x, ...)</code>
/// </summary>
public struct ParamBuilder
{
    string name;
    GTypeRef type;
    Value defValue;

    public override string ToString() => $"{type.FullName} {name}{(defValue.IsNull ? "" : " = " + defValue)}";

    public ParamBuilder(string name, GTypeRef type, Value defValue = default)
    {
        this.name = name;
        this.type = type;
        this.defValue = defValue;
    }
}