using System;

namespace MathSharp.Attributes
{
    // TODO for analyzer
    /// <summary>
    /// Indicates a type is desired to be a Homogeneous Float Aggregate type,
    /// which consists of up to and including 4 identical floating point types, either
    /// <see cref="float"/> or <see cref="double"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public class HfaAttribute : Attribute
    {

    }
}
