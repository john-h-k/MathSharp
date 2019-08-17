using OpenTK;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using Xunit;

// ReSharper disable ConvertToConstant.Local

//namespace MathSharp.UnitTests.VectorTests.VectorDouble.ConversionTests
//{
//    public class LoadTests
//    {
//        [Fact]
//        public void Load_StackVector4d_Passes()
//        {
//            var vector = new Vector4d(1, 2, 3, 4);

//            Vector256<double> loaded = vector.Load();
//            Assert.True(MathSharp.UnitTests.TestHelpers.AreEqual(vector, loaded));
//        }

//        [Fact]
//        public void Load_StackVector3d_Passes()
//        {
//            var vector = new Vector3d(1, 2, 3);

//            Vector256<double> loaded = vector.Load();
//            Assert.True(MathSharp.UnitTests.TestHelpers.AreEqual(vector, loaded));
//        }

//        [Fact]
//        public void Load_StackVector3dWithScalarW_Passes()
//        {
//            var vector = new Vector3d(1, 2, 3);
//            var scalar = 4324d;

//            Vector256<double> loaded = vector.Load(scalar);
//            Assert.True(MathSharp.UnitTests.TestHelpers.AreEqual(new Vector4d(vector, scalar), loaded));
//        }

//        [Fact]
//        public void Load_StackVector2d_Passes()
//        {
//            var vector = new Vector2d(1, 2);

//            Vector256<double> loaded = vector.Load();
//            Assert.True(MathSharp.UnitTests.TestHelpers.AreEqual(vector, loaded));
//        }

//        [Fact]
//        public void Load_StackVector2dBroadcasted_Passes()
//        {
//            var vector = new Vector2d(1, 2);

//            Vector256<double> loaded = vector.LoadBroadcast();
//            Assert.True(MathSharp.UnitTests.TestHelpers.AreEqual(new Vector4d(vector, vector.X, vector.Y), loaded));
//        }

//        [Fact]
//        public void Load_StackScalar_Passes()
//        {
//            var vector = 55d;

//            Vector256<double> loaded = vector.LoadScalar();
//            Assert.True(MathSharp.UnitTests.TestHelpers.AreEqual(vector, loaded));
//        }

//        [Fact]
//        public void Load_StackScalarBroadcasted_Passes()
//        {
//            var vector = 55d;

//            Vector256<double> loaded = vector.LoadScalarBroadcast();
//            Assert.True(MathSharp.UnitTests.TestHelpers.AreEqual(new Vector4d(vector), loaded));
//        }
//    }
//}