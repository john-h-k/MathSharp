using System;

namespace MathSharp.Attributes
{
    /// <summary>
    /// Indicates a type should be aligned on a certain boundary
    /// </summary>
    public class AlignedAttribute : Attribute
    {
        /// <summary>
        /// The ideal alignment for the type
        /// </summary>
        public int Alignment;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlignedAttribute"/> class with a specified alignment "<paramref name="alignment"/>"
        /// </summary>
        /// <param name="alignment">The ideal alignment for the type, a power of 2</param>
        public AlignedAttribute(int alignment)
        {
            Alignment = alignment;
        }
    }
}
