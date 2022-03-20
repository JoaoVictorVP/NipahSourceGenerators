using System;

namespace NipahSourceGenerators.Core;

[Flags]
public enum MemberModifier
{
    None = 0,
    Const = 1,
    ReadOnly = 2,
    Static = 4,
    Abstract = 8,
    Virtual = 16,
    Partial = 32,
    Unsafe = 64,
    Async = 128
}
