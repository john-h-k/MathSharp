using System;

namespace MathSharp.Attributes
{
    public class AlignedAttribute : Attribute
    {
        public int Alignment;

        public AlignedAttribute(int alignment)
        {
            if (alignment < 0)
                throw new ArgumentOutOfRangeException(nameof(alignment));
            Alignment = alignment;
        }
    }
}
