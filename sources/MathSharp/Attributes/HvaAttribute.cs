using System;
using System.Runtime.Intrinsics;

namespace MathSharp.Attributes
{
    // TODO for analyzer
    /// <summary>
    /// Indicates a type is desired to be a Homogeneous Vector Aggregate type,
    /// which consists of up to and including 4 identical vector types,
    /// where the vector types are <see cref="Vector64{T}"/>, <see cref="Vector128{T}"/>, and <see cref="Vector256{T}"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public class HvaAttribute : Attribute
    {
        
    }
}
