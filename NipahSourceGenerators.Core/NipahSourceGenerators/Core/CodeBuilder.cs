using NipahSourceGenerators.Core.CSharp;
using System;
using System.Collections.Generic;
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

    public TypeBuilder Class(string name, MemberVisibility visibily = MemberVisibility.Public, MemberModifier modifiers = MemberModifier.None, GTypeRef baseType = default, params GTypeRef[] interfaces) => type(name, "class", visibily, modifiers, baseType, interfaces);
    public TypeBuilder Struct(string name, MemberVisibility visibily = MemberVisibility.Public, MemberModifier modifiers = MemberModifier.None, GTypeRef baseType = default, params GTypeRef[] interfaces) => type(name, "struct", visibily, modifiers, baseType, interfaces);

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
    TypeBuilder type(string name, string kind, MemberVisibility visibily = MemberVisibility.Public, MemberModifier modifiers = MemberModifier.None, GTypeRef baseType = default, Span<GTypeRef> interfaces = default)
    {
        switch (modifiers)
        {
            case MemberModifier.Const: throw new Exception("Types cannot be constant");
            case MemberModifier.ReadOnly: throw new Exception("Types cannot be readonly");
            case MemberModifier.Virtual: throw new Exception("Types cannot be virtual");
        }

        code.Append($"{visibily.ToCS()}{modifiers.ToCS()}{kind} {name}");

        bool hasBaseType = baseType.IsNull is false;
        bool hasInterfaces = interfaces is Span<GTypeRef> { Length: > 0 };

        if (hasBaseType || hasInterfaces)
            code.Append(" : ");

        if (hasBaseType)
            code.Append($"{baseType.FullName} ");
        
        if(hasInterfaces)
        {
            bool first = true;
            foreach(var inter in interfaces)
            {
                if (!first)
                    code.Append(", ");
                else
                    first = false;
                code.Append(inter.FullName);
            }
        }

        code.AppendLine("\n{");

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

    public MethodBuilder Return(Value value)
    {
        code.AppendLine($"return {value};");
        return this;
    }

    public MethodBuilder NonInitializedLocal(string name, GTypeRef type)
    {
        code.AppendLine($"{type.FullName} {name};");
        return this;
    }

    public MethodBuilder Local(string name, GTypeRef type, Value value)
    {
        code.AppendLine($"{type.FullName} {name} = {value};");
        return this;
    }

    public MethodBuilder Bind(string variable, Value value)
    {
        code.AppendLine($"{variable} = {value};");
        return this;
    }

    public MethodBuilder Invoke(GTypeRef type, string methodName, params Value[] args)
    {
        code.AppendLine(new InvokeBuilder(type, methodName, args).ToString() + ";");
        return this;
    }
    public MethodBuilder Invoke(string path, params Value[] args)
    {
        code.AppendLine(new InvokeBuilder(path, args).ToString() + ";");
        return this;
    }
    public MethodBuilder InvokeAsync(string path, params Value[] args)
    {
        code.AppendLine(new InvokeBuilder("await " + path, args).ToString() + ";");
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
    readonly bool isNew;
    readonly Span<string> parts;
    readonly Span<Value> args;
    bool isAwait;

    public InvokeBuilder Await()
    {
        if (isNew)
            throw new Exception("Cannot await a constructor");

        isAwait = true;

        return this;
    }

    public override string ToString()
    {
        string invoke = "";

        if (isNew)
            invoke = "new ";
        else if (isAwait)
            invoke = "await ";

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

    public InvokeBuilder(GTypeRef type, params Value[] args)
    {
        this.isNew = true;
        this.parts = new[] { type.FullName };
        this.args = args;

        isAwait = false;
    }

    public InvokeBuilder(string parts, Span<Value> args, bool isNew = false)
    {
        this.isNew = isNew;
        this.parts = new[] { parts };
        this.args = args;

        isAwait = false;
    }
    public InvokeBuilder(string parts, bool isNew, params Value[] args)
    {
        this.isNew = isNew;
        this.parts = new[] { parts };
        this.args = args;

        isAwait = false;
    }

    public InvokeBuilder(Span<string> parts, Span<Value> args, bool isNew = false)
    {
        this.isNew = isNew;
        this.parts = parts;
        this.args = args;

        isAwait = false;
    }

    public InvokeBuilder(Span<string> parts, params Value[] args)
    {
        this.isNew = false;
        this.parts = parts;
        this.args = args;

        isAwait = false;
    }
    public InvokeBuilder(Span<string> parts, bool isNew, params Value[] args)
    {
        this.isNew = isNew;
        this.parts = parts;
        this.args = args;

        isAwait = false;
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
        this.isNew = false;
        parts = new[] { type.FullName, methodName };
        this.args = args;

        isAwait = false;
    }
    public InvokeBuilder(GTypeRef type, string methodName, bool isNew, params Value[] args)
    {
        this.isNew = isNew;
        parts = new[] { type.FullName, methodName };
        this.args = args;

        isAwait = false;
    }
}
/// <summary>
/// Parameter, like: <code>(int x, ...)</code>
/// </summary>
public struct ParamBuilder
{
    public string Name => name;
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
/*public ref struct Span<T>
{
    public T this[int index]
    {
        get => array[index];
        set => array[index] = value;
    }

    public readonly int Length;
    T[] array;

    public static implicit operator Span<T>(T[] array) => new Span<T>(array);

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in array)
            yield return item;
    }

    public Span(T[] array)
    {
        this.array = array;
        Length = array.Length;
    }
}*/