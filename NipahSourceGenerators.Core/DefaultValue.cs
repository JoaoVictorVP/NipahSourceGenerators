namespace NipahSourceGenerators.Core;

public struct Value
{
    public static readonly Value Null = new Value { str = "null", constOnly = true };

    string str = null;
    bool constOnly = false;
    public bool IsNull => str is null;

    public bool IsConst => constOnly;

    public override string ToString() => str;

    /// <summary>
    /// Constructor for default value that accepts source code instead of a string literal (for cases where you want to call a method of something similar
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Value Source(string source) => new Value { str = source };

    public static implicit operator Value(string literal) => new Value(literal);
    public static implicit operator string(Value value) => value.str;

    public static implicit operator Value(int int32) => new Value(int32);
    public static implicit operator Value(long int64) => new Value(int64);
    public static implicit operator Value(float float32) => new Value(float32);
    public static implicit operator Value(double float64) => new Value(float64);
    public static implicit operator Value(decimal dec) => new Value(dec);
    public static implicit operator Value(bool boolean) => new Value(boolean);

    public static implicit operator Value(DBNull @null) => Null;

    public static implicit operator Value(InvokeBuilder invoke) => new Value(invoke);

    void set(string str, bool constOnly = false)
    {
        this.str = str;
        this.constOnly = constOnly;
    }

    public Value(string literal) => set("\"" + literal + "\"", true);
    public Value(int int32) => set(int32.ToString(), true);
    public Value(long int64) => set(int64.ToString() + "l", true);
    public Value(float float32) => set(float32.ToString() + "f", true);
    public Value(double float64) => set(float64.ToString() + "d", true);
    public Value(decimal dec) => set(dec.ToString() + "d", true);
    public Value(bool boolean) => set(boolean switch { true => "true", false => "false" }, true);

    public Value(InvokeBuilder invoke) => set(invoke.ToString());
}