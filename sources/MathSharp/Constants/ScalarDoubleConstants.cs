using System;

namespace MathSharp.Constants
{
    /// <summary>
    /// <see cref="double"/> constants used frequently in maths
    /// </summary>
    public static class ScalarDoubleConstants
    {
        /// <summary>
        /// Represents the ratio of the circumference of a circle to its diameter, specified by the constant, π
        /// </summary>
        public const double Pi = Math.PI; // for symmetry

        /// <summary>
        /// π multiplied by 2
        /// </summary>
        public const double Pi2 = Pi * 2d;

        /// <summary>
        /// The reciprocal of π, 1 divided by π
        /// </summary>
        public const double OneDivPi = 1d / Pi;

        /// <summary>
        /// The reciprocal of 2π, 1 divided by 2π
        /// </summary>
        public const double OneDiv2Pi = 1d / Pi2;

        /// <summary>
        /// π divided by 2
        /// </summary>
        public const double PiDiv2 = Pi / 2d;

        /// <summary>
        /// π divided by 4
        /// </summary>
        public const double PiDiv4 = Pi / 4d;
    }
}
