using System;

namespace MathSharp.Constants
{
    /// <summary>
    /// <see cref="float"/> constants used frequently in maths
    /// </summary>
    public static class ScalarSingleConstants
    {
        /// <summary>
        /// Represents the ratio of the circumference of a circle to its diameter, specified by the constant, π
        /// </summary>
        public const float Pi = MathF.PI; // for symmetry

        /// <summary>
        /// π multiplied by 2
        /// </summary>
        public const float Pi2 = Pi * 2f;

        /// <summary>
        /// The reciprocal of π, 1 divided by π
        /// </summary>
        public const float OneDivPi = 1f / Pi;

        /// <summary>
        /// The reciprocal of 2π, 1 divided by 2π
        /// </summary>
        public const float OneDiv2Pi = 1f / Pi2;

        /// <summary>
        /// π divided by 2
        /// </summary>
        public const float PiDiv2 = Pi / 2f;

        /// <summary>
        /// π divided by 4
        /// </summary>
        public const float PiDiv4 = Pi / 4f;
    }
}
