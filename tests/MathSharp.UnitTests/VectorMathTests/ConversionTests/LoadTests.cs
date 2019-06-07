using System.Numerics;
using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;
using Xunit;
// ReSharper disable ConvertToConstant.Local

namespace MathSharp.UnitTests.VectorMathTests
{
    public class LoadTests
    {
        [Fact]
        public void Load_StackVector4_Passes()
        {
            var vector = new Vector4(1, 2, 3, 4);

            Vector128<float> loaded = vector.Load();
            Assert.True(AreEqual(vector, loaded));
        }

        [Fact]
        public void Load_StackVector3_Passes()
        {
            var vector = new Vector3(1, 2, 3);

            Vector128<float> loaded = vector.Load();
            Assert.True(AreEqual(vector, loaded));
        }

        [Fact]
        public void Load_StackVector3WithScalarW_Passes()
        {
            var vector = new Vector3(1, 2, 3);
            var scalar = 4324f;

            Vector128<float> loaded = vector.Load(scalar);
            Assert.True(AreEqual(new Vector4(vector, scalar), loaded));
        }

        [Fact]
        public void Load_StackVector2_Passes()
        {
            var vector = new Vector2(1, 2);

            Vector128<float> loaded = vector.Load();
            Assert.True(AreEqual(vector, loaded));
        }

        [Fact]
        public void Load_StackVector2Broadcasted_Passes()
        {
            var vector = new Vector2(1, 2);

            Vector128<float> loaded = vector.LoadBroadcast();
            Assert.True(AreEqual(new Vector4(vector, vector.X, vector.Y), loaded));
        }

        [Fact]
        public void Load_StackScalar_Passes()
        {
            var vector = 55f;

            Vector128<float> loaded = vector.LoadScalar();
            Assert.True(AreEqual(vector, loaded));
        }

        [Fact]
        public void Load_StackScalarBroadcasted_Passes()
        {
            var vector = 55f;

            Vector128<float> loaded = vector.LoadScalarBroadcast();
            Assert.True(AreEqual(new Vector4(vector), loaded));
        }
    }
}