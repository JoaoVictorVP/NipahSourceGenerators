namespace NipahSourceGenerators.Core;

public class GField : GMember
{
    public GTypeRef FieldType;
    public string DefaultValue;

    public GField WithDefaultValue(string defValue!!)
    {
        DefaultValue = defValue;
        return this;
    }
}
