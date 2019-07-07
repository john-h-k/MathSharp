using System;

namespace MathSharp.Attributes
{
    public class AlignedAttribute : Attribute
    {
        public int Alignment;

        public AlignedAttribute(int alignment)
        {
            Alignment = alignment;
        }
    }
}
