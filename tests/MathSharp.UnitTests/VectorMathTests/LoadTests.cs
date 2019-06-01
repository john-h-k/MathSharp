using System.Numerics;
using System.Runtime.Intrinsics;
using MathSharp;
using Xunit;
using static MathSharp.Helpers;

namespace MathSharp.Tests.VectorMathTests
{
    public class LoadTests
    {
        [Fact]
        public void Load_StackVector4_Passes()
        {
            var vector = new Vector4();

            Vector128<float> loaded = vector.Load();
            Assert.True(AreEqual(vector, loaded));
        }

        [Fact]
        public Vector128<float> Load_StackVector4_Passes2()
        {
            var vector = new Vector4();

            return vector.Load();
        }
    }
}