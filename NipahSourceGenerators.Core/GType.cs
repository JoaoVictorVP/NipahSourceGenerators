namespace NipahSourceGenerators.Core;

public class GType<TField, TProperty, TMethod> : GMember where TField : GField where TProperty : GProperty where TMethod : GMethod
{
    public GTypeRef BaseType;

    public List<GTypeRef> Interfaces = new List<GTypeRef>(32);

    public List<TField> Fields = new (32);
    public List<TProperty> Properties = new(32);
    public List<TMethod> Methods = new(32);

    public string Namespace;

    public string FullName => string.IsNullOrEmpty(Namespace) ? Name : $"{Namespace}.{Name}";

    public GType<TField, TProperty, TMethod> InNamespace(string @namespace)
    {
        Namespace = @namespace;
        return this;
    }

    public GType<TField, TProperty, TMethod> ImplInterface(GTypeRef @interface)
    {
        Interfaces.Add(@interface);
        return this;
    }

    public GType<TField, TProperty, TMethod> WithField(TField field)
    {
        Fields.Add(field);
        return this;
    }
    public GType<TField, TProperty, TMethod> WithProperty(TProperty property)
    {
        Properties.Add(property);
        return this;
    }
    public GType<TField, TProperty, TMethod> WithMethod(TMethod method)
    {
        Methods.Add(method);
        return this;
    }

    public GMember WithBaseType(GTypeRef baseType)
    {
        BaseType = baseType;
        return this;
    }
}
