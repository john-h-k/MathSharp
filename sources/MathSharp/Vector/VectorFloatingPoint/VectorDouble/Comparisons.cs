using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{
    
    

    public static partial class Vector
    {
        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareEqual(Vector256<double> left, Vector256<double> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Compare(left, right, FloatComparisonMode.OrderedEqualNonSignaling);
            }

            return CompareEqual_Software(left, right);
        }
        
        
        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareNotEqual(Vector256<double> left, Vector256<double> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Compare(left, right, FloatComparisonMode.UnorderedNotEqualNonSignaling);
            }

            return CompareNotEqual_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareGreaterThan(Vector256<double> left, Vector256<double> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Compare(left, right, FloatComparisonMode.UnorderedNotLessThanOrEqualNonSignaling);
            }

            return CompareGreaterThan_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareLessThan(Vector256<double> left, Vector256<double> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Compare(left, right, FloatComparisonMode.OrderedLessThanSignaling);
            }

            return CompareLessThan_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareGreaterThanOrEqual(Vector256<double> left, Vector256<double> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Compare(left, right, FloatComparisonMode.UnorderedNotLessThanSignaling);
            }

            return CompareGreaterThanOrEqual_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareLessThanOrEqual(Vector256<double> left, Vector256<double> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Compare(left, right, FloatComparisonMode.OrderedLessThanOrEqualSignaling);
            }

            return CompareLessThanOrEqual_Software(left, right);
        }
    }
}
