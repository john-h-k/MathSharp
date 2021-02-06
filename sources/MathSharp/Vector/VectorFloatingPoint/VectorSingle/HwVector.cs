using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using System.Text;

namespace MathSharp.Vector.VectorFloatingPoint.VectorSingle
{
    public readonly struct HwVector
    {
        private readonly Vector128<float> Value;

        public static HwVector operator + (HwVector left, HwVector right)
        {
            return Vector.Add(left, right);
        }
    }
}
