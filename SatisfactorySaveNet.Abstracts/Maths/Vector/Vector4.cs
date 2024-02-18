using SatisfactorySaveNet.Abstracts.Maths.Data;
using SatisfactorySaveNet.Abstracts.Maths.Matrix;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace SatisfactorySaveNet.Abstracts.Maths.Vector
{
    /// <summary>
    /// Represents a 4D vector using four single-precision floating-point numbers.
    /// </summary>
    /// <remarks>
    /// The Vector4 structure is suitable for interoperation with unmanaged code requiring four consecutive floats.
    /// </remarks>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4 : IEquatable<Vector4>, IFormattable
    {
        /// <summary>
        /// The X component of the Vector4.
        /// </summary>
        public float X;

        /// <summary>
        /// The Y component of the Vector4.
        /// </summary>
        public float Y;

        /// <summary>
        /// The Z component of the Vector4.
        /// </summary>
        public float Z;

        /// <summary>
        /// The W component of the Vector4.
        /// </summary>
        public float W;

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the X-axis.
        /// </summary>
        public static readonly Vector4 UnitX = new(1, 0, 0, 0);

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the Y-axis.
        /// </summary>
        public static readonly Vector4 UnitY = new(0, 1, 0, 0);

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the Z-axis.
        /// </summary>
        public static readonly Vector4 UnitZ = new(0, 0, 1, 0);

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the W-axis.
        /// </summary>
        public static readonly Vector4 UnitW = new(0, 0, 0, 1);

        /// <summary>
        /// Defines an instance with all components set to 0.
        /// </summary>
        public static readonly Vector4 Zero = new(0, 0, 0, 0);

        /// <summary>
        /// Defines an instance with all components set to 1.
        /// </summary>
        public static readonly Vector4 One = new(1, 1, 1, 1);

        /// <summary>
        /// Defines an instance with all components set to positive infinity.
        /// </summary>
        public static readonly Vector4 PositiveInfinity = new(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

        /// <summary>
        /// Defines an instance with all components set to negative infinity.
        /// </summary>
        public static readonly Vector4 NegativeInfinity = new(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

        /// <summary>
        /// Defines the size of the Vector4 struct in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Unsafe.SizeOf<Vector4>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> struct.
        /// </summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public Vector4(float value)
        {
            X = value;
            Y = value;
            Z = value;
            W = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> struct.
        /// </summary>
        /// <param name="x">The x component of the Vector4.</param>
        /// <param name="y">The y component of the Vector4.</param>
        /// <param name="z">The z component of the Vector4.</param>
        /// <param name="w">The w component of the Vector4.</param>
        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> struct.
        /// </summary>
        /// <param name="v">The Vector2 to copy components from.</param>
        public Vector4(Vector2 v)
        {
            X = v.X;
            Y = v.Y;
            Z = 0.0f;
            W = 0.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> struct.
        /// </summary>
        /// <param name="v">The Vector3 to copy components from.</param>
        /// <remarks>
        ///  .<seealso cref="Vector4(Vector3, float)"/>
        /// </remarks>
        public Vector4(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = 0.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> struct.
        /// </summary>
        /// <param name="v">The Vector3 to copy components from.</param>
        /// <param name="w">The w component of the new Vector4.</param>
        public Vector4(Vector3 v, float w)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> struct.
        /// </summary>
        /// <param name="v">The Vector4 to copy components from.</param>
        public Vector4(Vector4 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = v.W;
        }

        /// <summary>
        /// Gets or sets the value at the index of the Vector.
        /// </summary>
        /// <param name="index">The index of the component from the Vector.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index is less than 0 or greater than 3.</exception>
        public float this[int index]
        {
            readonly get
            {
                if (index == 0)
                {
                    return X;
                }

                if (index == 1)
                {
                    return Y;
                }

                var tmp = index == 2 ? Z : index;

#pragma warning disable S112 // General or reserved exceptions should never be thrown
                return tmp == 3 ? W : throw new IndexOutOfRangeException("You tried to access this vector at index: " + index);
#pragma warning restore S112 // General or reserved exceptions should never be thrown
            }

            set
            {
                if (index == 0)
                {
                    X = value;
                }
                else if (index == 1)
                {
                    Y = value;
                }
                else if (index == 2)
                {
                    Z = value;
                }
                else
                {
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                    W = index == 3 ? value : throw new IndexOutOfRangeException("You tried to set this vector at index: " + index);
#pragma warning restore S112 // General or reserved exceptions should never be thrown
                }
            }
        }

        /// <summary>
        /// Gets the length (magnitude) of the vector.
        /// </summary>
        /// <see cref="LengthFast"/>
        /// <seealso cref="LengthSquared"/>
        public readonly float Length => MathF.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));

        /// <summary>
        /// Gets an approximation of the vector length (magnitude).
        /// </summary>
        /// <remarks>
        /// This property uses an approximation of the square root function to calculate vector magnitude, with
        /// an upper error bound of 0.001.
        /// </remarks>
        /// <see cref="Length"/>
        /// <seealso cref="LengthSquared"/>
        public readonly float LengthFast => 1.0f / MathHelper.InverseSqrtFast((X * X) + (Y * Y) + (Z * Z) + (W * W));

        /// <summary>
        /// Gets the square of the vector length (magnitude).
        /// </summary>
        /// <remarks>
        /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
        /// for comparisons.
        /// </remarks>
        /// <see cref="Length"/>
        /// <seealso cref="LengthFast"/>
        public readonly float LengthSquared => (X * X) + (Y * Y) + (Z * Z) + (W * W);

        /// <summary>
        /// Returns a copy of the Vector4 scaled to unit length.
        /// </summary>
        /// <returns>The normalized copy.</returns>
        public readonly Vector4 Normalized()
        {
            Vector4 v = this;
            v.Normalize();
            return v;
        }

        /// <summary>
        /// Scales the Vector4 to unit length.
        /// </summary>
        public void Normalize()
        {
            float scale = 1.0f / Length;
            X *= scale;
            Y *= scale;
            Z *= scale;
            W *= scale;
        }

        /// <summary>
        /// Scales the Vector4 to approximately unit length.
        /// </summary>
        public void NormalizeFast()
        {
            float scale = MathHelper.InverseSqrtFast((X * X) + (Y * Y) + (Z * Z) + (W * W));
            X *= scale;
            Y *= scale;
            Z *= scale;
            W *= scale;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Result of operation.</returns>
        [Pure]
        public static Vector4 Add(Vector4 a, Vector4 b)
        {
            Add(in a, in b, out a);
            return a;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <param name="result">Result of operation.</param>
        public static void Add(in Vector4 a, in Vector4 b, out Vector4 result)
        {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
            result.Z = a.Z + b.Z;
            result.W = a.W + b.W;
        }

        /// <summary>
        /// Subtract one Vector from another.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>Result of subtraction.</returns>
        [Pure]
        public static Vector4 Subtract(Vector4 a, Vector4 b)
        {
            Subtract(in a, in b, out a);
            return a;
        }

        /// <summary>
        /// Subtract one Vector from another.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Result of subtraction.</param>
        public static void Subtract(in Vector4 a, in Vector4 b, out Vector4 result)
        {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
            result.Z = a.Z - b.Z;
            result.W = a.W - b.W;
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static Vector4 Multiply(Vector4 vector, float scale)
        {
            Multiply(in vector, scale, out vector);
            return vector;
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Multiply(in Vector4 vector, float scale, out Vector4 result)
        {
            result.X = vector.X * scale;
            result.Y = vector.Y * scale;
            result.Z = vector.Z * scale;
            result.W = vector.W * scale;
        }

        /// <summary>
        /// Multiplies a vector by the components a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static Vector4 Multiply(Vector4 vector, Vector4 scale)
        {
            Multiply(in vector, in scale, out vector);
            return vector;
        }

        /// <summary>
        /// Multiplies a vector by the components of a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Multiply(in Vector4 vector, in Vector4 scale, out Vector4 result)
        {
            result.X = vector.X * scale.X;
            result.Y = vector.Y * scale.Y;
            result.Z = vector.Z * scale.Z;
            result.W = vector.W * scale.W;
        }

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static Vector4 Divide(Vector4 vector, float scale)
        {
            Divide(in vector, scale, out vector);
            return vector;
        }

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Divide(in Vector4 vector, float scale, out Vector4 result)
        {
            result.X = vector.X / scale;
            result.Y = vector.Y / scale;
            result.Z = vector.Z / scale;
            result.W = vector.W / scale;
        }

        /// <summary>
        /// Divides a vector by the components of a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static Vector4 Divide(Vector4 vector, Vector4 scale)
        {
            Divide(in vector, in scale, out vector);
            return vector;
        }

        /// <summary>
        /// Divide a vector by the components of a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Divide(in Vector4 vector, in Vector4 scale, out Vector4 result)
        {
            result.X = vector.X / scale.X;
            result.Y = vector.Y / scale.Y;
            result.Z = vector.Z / scale.Z;
            result.W = vector.W / scale.W;
        }

        /// <summary>
        /// Returns a vector created from the smallest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>The component-wise minimum.</returns>
        [Pure]
        public static Vector4 ComponentMin(Vector4 a, Vector4 b)
        {
            a.X = a.X < b.X ? a.X : b.X;
            a.Y = a.Y < b.Y ? a.Y : b.Y;
            a.Z = a.Z < b.Z ? a.Z : b.Z;
            a.W = a.W < b.W ? a.W : b.W;
            return a;
        }

        /// <summary>
        /// Returns a vector created from the smallest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">The component-wise minimum.</param>
        public static void ComponentMin(in Vector4 a, in Vector4 b, out Vector4 result)
        {
            result.X = a.X < b.X ? a.X : b.X;
            result.Y = a.Y < b.Y ? a.Y : b.Y;
            result.Z = a.Z < b.Z ? a.Z : b.Z;
            result.W = a.W < b.W ? a.W : b.W;
        }

        /// <summary>
        /// Returns a vector created from the largest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>The component-wise maximum.</returns>
        [Pure]
        public static Vector4 ComponentMax(Vector4 a, Vector4 b)
        {
            a.X = a.X > b.X ? a.X : b.X;
            a.Y = a.Y > b.Y ? a.Y : b.Y;
            a.Z = a.Z > b.Z ? a.Z : b.Z;
            a.W = a.W > b.W ? a.W : b.W;
            return a;
        }

        /// <summary>
        /// Returns a vector created from the largest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">The component-wise maximum.</param>
        public static void ComponentMax(in Vector4 a, in Vector4 b, out Vector4 result)
        {
            result.X = a.X > b.X ? a.X : b.X;
            result.Y = a.Y > b.Y ? a.Y : b.Y;
            result.Z = a.Z > b.Z ? a.Z : b.Z;
            result.W = a.W > b.W ? a.W : b.W;
        }

        /// <summary>
        /// Returns the Vector4 with the minimum magnitude. If the magnitudes are equal, the second vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>The minimum Vector4.</returns>
        [Pure]
        public static Vector4 MagnitudeMin(Vector4 left, Vector4 right) => left.LengthSquared < right.LengthSquared ? left : right;

        /// <summary>
        /// Returns the Vector4 with the minimum magnitude. If the magnitudes are equal, the second vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <param name="result">The magnitude-wise minimum.</param>
        public static void MagnitudeMin(in Vector4 left, in Vector4 right, out Vector4 result)
        {
            result = left.LengthSquared < right.LengthSquared ? left : right;
        }

        /// <summary>
        /// Returns the Vector4 with the maximum magnitude. If the magnitudes are equal, the first vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>The maximum Vector4.</returns>
        [Pure]
        public static Vector4 MagnitudeMax(Vector4 left, Vector4 right) => left.LengthSquared >= right.LengthSquared ? left : right;

        /// <summary>
        /// Returns the Vector4 with the maximum magnitude. If the magnitudes are equal, the first vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <param name="result">The magnitude-wise maximum.</param>
        public static void MagnitudeMax(in Vector4 left, in Vector4 right, out Vector4 result)
        {
            result = left.LengthSquared >= right.LengthSquared ? left : right;
        }

        /// <summary>
        /// Clamp a vector to the given minimum and maximum vectors.
        /// </summary>
        /// <param name="vec">Input vector.</param>
        /// <param name="min">Minimum vector.</param>
        /// <param name="max">Maximum vector.</param>
        /// <returns>The clamped vector.</returns>
        [Pure]
        public static Vector4 Clamp(Vector4 vec, Vector4 min, Vector4 max)
        {
            var tmpX = vec.X < min.X ? min.X : vec.X;
            vec.X = tmpX > max.X ? max.X : vec.X;
            var tmpY = vec.Y < min.Y ? min.Y : vec.Y;
            vec.Y = tmpY > max.Y ? max.Y : vec.Y;
            var tmpZ = vec.Z < min.Z ? min.Z : vec.Z;
            vec.Z = tmpZ > max.Z ? max.Z : vec.Z;
            var tmpW = vec.W < min.W ? min.W : vec.W;
            vec.W = tmpW > max.W ? max.W : vec.W;
            return vec;
        }

        /// <summary>
        /// Clamp a vector to the given minimum and maximum vectors.
        /// </summary>
        /// <param name="vec">Input vector.</param>
        /// <param name="min">Minimum vector.</param>
        /// <param name="max">Maximum vector.</param>
        /// <param name="result">The clamped vector.</param>
        public static void Clamp(in Vector4 vec, in Vector4 min, in Vector4 max, out Vector4 result)
        {
            var tmpX = vec.X < min.X ? min.X : vec.X;
            result.X = tmpX > max.X ? max.X : vec.X;
            var tmpY = vec.Y < min.Y ? min.Y : vec.Y;
            result.Y = tmpY > max.Y ? max.Y : vec.Y;
            var tmpZ = vec.Z < min.Z ? min.Z : vec.Z;
            result.Z = tmpZ > max.Z ? max.Z : vec.Z;
            var tmpW = vec.W < min.W ? min.W : vec.W;
            result.W = tmpW > max.W ? max.W : vec.W;
        }

        /// <summary>
        /// Scale a vector to unit length.
        /// </summary>
        /// <param name="vec">The input vector.</param>
        /// <returns>The normalized copy.</returns>
        [Pure]
        public static Vector4 Normalize(Vector4 vec)
        {
            float scale = 1.0f / vec.Length;
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        /// <summary>
        /// Scale a vector to unit length.
        /// </summary>
        /// <param name="vec">The input vector.</param>
        /// <param name="result">The normalized vector.</param>
        public static void Normalize(in Vector4 vec, out Vector4 result)
        {
            float scale = 1.0f / vec.Length;
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
            result.Z = vec.Z * scale;
            result.W = vec.W * scale;
        }

        /// <summary>
        /// Scale a vector to approximately unit length.
        /// </summary>
        /// <param name="vec">The input vector.</param>
        /// <returns>The normalized copy.</returns>
        [Pure]
        public static Vector4 NormalizeFast(Vector4 vec)
        {
            float scale = MathHelper.InverseSqrtFast((vec.X * vec.X) + (vec.Y * vec.Y) + (vec.Z * vec.Z) + (vec.W * vec.W));
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        /// <summary>
        /// Scale a vector to approximately unit length.
        /// </summary>
        /// <param name="vec">The input vector.</param>
        /// <param name="result">The normalized copy.</param>
        public static void NormalizeFast(in Vector4 vec, out Vector4 result)
        {
            float scale = MathHelper.InverseSqrtFast((vec.X * vec.X) + (vec.Y * vec.Y) + (vec.Z * vec.Z) + (vec.W * vec.W));
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
            result.Z = vec.Z * scale;
            result.W = vec.W * scale;
        }

        /// <summary>
        /// Calculate the dot product of two vectors.
        /// </summary>
        /// <param name="left">First operand.</param>
        /// <param name="right">Second operand.</param>
        /// <returns>The dot product of the two inputs.</returns>
        [Pure]
        public static float Dot(Vector4 left, Vector4 right) => (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);

        /// <summary>
        /// Calculate the dot product of two vectors.
        /// </summary>
        /// <param name="left">First operand.</param>
        /// <param name="right">Second operand.</param>
        /// <param name="result">The dot product of the two inputs.</param>
        public static void Dot(in Vector4 left, in Vector4 right, out float result)
        {
            result = (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
        }

        /// <summary>
        /// Returns a new vector that is the linear blend of the 2 given vectors.
        /// </summary>
        /// <param name="a">First input vector.</param>
        /// <param name="b">Second input vector.</param>
        /// <param name="blend">The blend factor.</param>
        /// <returns>a when blend=0, b when blend=1, and a linear combination otherwise.</returns>
        [Pure]
        public static Vector4 Lerp(Vector4 a, Vector4 b, float blend)
        {
            a.X = (blend * (b.X - a.X)) + a.X;
            a.Y = (blend * (b.Y - a.Y)) + a.Y;
            a.Z = (blend * (b.Z - a.Z)) + a.Z;
            a.W = (blend * (b.W - a.W)) + a.W;
            return a;
        }

        /// <summary>
        /// Returns a new vector that is the linear blend of the 2 given vectors.
        /// </summary>
        /// <param name="a">First input vector.</param>
        /// <param name="b">Second input vector.</param>
        /// <param name="blend">The blend factor.</param>
        /// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise.</param>
        public static void Lerp(in Vector4 a, in Vector4 b, float blend, out Vector4 result)
        {
            result.X = (blend * (b.X - a.X)) + a.X;
            result.Y = (blend * (b.Y - a.Y)) + a.Y;
            result.Z = (blend * (b.Z - a.Z)) + a.Z;
            result.W = (blend * (b.W - a.W)) + a.W;
        }

        /// <summary>
        /// Returns a new vector that is the component-wise linear blend of the 2 given vectors.
        /// </summary>
        /// <param name="a">First input vector.</param>
        /// <param name="b">Second input vector.</param>
        /// <param name="blend">The blend factor.</param>
        /// <returns>a when blend=0, b when blend=1, and a component-wise linear combination otherwise.</returns>
        [Pure]
        public static Vector4 Lerp(Vector4 a, Vector4 b, Vector4 blend)
        {
            a.X = (blend.X * (b.X - a.X)) + a.X;
            a.Y = (blend.Y * (b.Y - a.Y)) + a.Y;
            a.Z = (blend.Z * (b.Z - a.Z)) + a.Z;
            a.W = (blend.W * (b.W - a.W)) + a.W;
            return a;
        }

        /// <summary>
        /// Returns a new vector that is the component-wise linear blend of the 2 given vectors.
        /// </summary>
        /// <param name="a">First input vector.</param>
        /// <param name="b">Second input vector.</param>
        /// <param name="blend">The blend factor.</param>
        /// <param name="result">a when blend=0, b when blend=1, and a component-wise linear combination otherwise.</param>
        public static void Lerp(in Vector4 a, in Vector4 b, Vector4 blend, out Vector4 result)
        {
            result.X = (blend.X * (b.X - a.X)) + a.X;
            result.Y = (blend.Y * (b.Y - a.Y)) + a.Y;
            result.Z = (blend.Z * (b.Z - a.Z)) + a.Z;
            result.W = (blend.W * (b.W - a.W)) + a.W;
        }

        /// <summary>
        /// Interpolate 3 Vectors using Barycentric coordinates.
        /// </summary>
        /// <param name="a">First input Vector.</param>
        /// <param name="b">Second input Vector.</param>
        /// <param name="c">Third input Vector.</param>
        /// <param name="u">First Barycentric Coordinate.</param>
        /// <param name="v">Second Barycentric Coordinate.</param>
        /// <returns>a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise.</returns>
        [Pure]
        public static Vector4 BaryCentric(Vector4 a, Vector4 b, Vector4 c, float u, float v)
        {
            BaryCentric(in a, in b, in c, u, v, out Vector4 result);
            return result;
        }

        /// <summary>
        /// Interpolate 3 Vectors using Barycentric coordinates.
        /// </summary>
        /// <param name="a">First input Vector.</param>
        /// <param name="b">Second input Vector.</param>
        /// <param name="c">Third input Vector.</param>
        /// <param name="u">First Barycentric Coordinate.</param>
        /// <param name="v">Second Barycentric Coordinate.</param>
        /// <param name="result">
        /// Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c
        /// otherwise.
        /// </param>
        public static void BaryCentric
        (
            in Vector4 a,
            in Vector4 b,
            in Vector4 c,
            float u,
            float v,
            out Vector4 result
        )
        {
#pragma warning disable S2234 // Arguments should be passed in the same order as the method parameters
            Subtract(in b, in a, out Vector4 ab);
#pragma warning restore S2234 // Arguments should be passed in the same order as the method parameters
            Multiply(in ab, u, out Vector4 abU);
            Add(in a, in abU, out Vector4 uPos);

            Subtract(in c, in a, out Vector4 ac);
            Multiply(in ac, v, out Vector4 acV);
            Add(in uPos, in acV, out result);
        }

        /// <summary>
        /// Transform a Vector by the given Matrix.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="mat">The desired transformation.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector4 TransformRow(Vector4 vec, Matrix4 mat)
        {
            TransformRow(in vec, in mat, out Vector4 result);
            return result;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="result">The transformed vector.</param>
        public static void TransformRow(in Vector4 vec, in Matrix4 mat, out Vector4 result)
        {
            result = new Vector4(
                (vec.X * mat.Row0.X) + (vec.Y * mat.Row1.X) + (vec.Z * mat.Row2.X) + (vec.W * mat.Row3.X),
                (vec.X * mat.Row0.Y) + (vec.Y * mat.Row1.Y) + (vec.Z * mat.Row2.Y) + (vec.W * mat.Row3.Y),
                (vec.X * mat.Row0.Z) + (vec.Y * mat.Row1.Z) + (vec.Z * mat.Row2.Z) + (vec.W * mat.Row3.Z),
                (vec.X * mat.Row0.W) + (vec.Y * mat.Row1.W) + (vec.Z * mat.Row2.W) + (vec.W * mat.Row3.W));
        }

        /// <summary>
        /// Transforms a vector by a quaternion rotation.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <returns>The result of the operation.</returns>
        [Pure]
        public static Vector4 Transform(Vector4 vec, Quaternion quat)
        {
            Transform(in vec, in quat, out Vector4 result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by a quaternion rotation.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <param name="result">The result of the operation.</param>
        public static void Transform(in Vector4 vec, in Quaternion quat, out Vector4 result)
        {
            Quaternion v = new(vec.X, vec.Y, vec.Z, vec.W);
            Quaternion.Invert(in quat, out Quaternion i);
            Quaternion.Multiply(in quat, in v, out Quaternion t);
            Quaternion.Multiply(in t, in i, out v);

            result.X = v.X;
            result.Y = v.Y;
            result.Z = v.Z;
            result.W = v.W;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix using right-handed notation.
        /// </summary>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector4 TransformColumn(Matrix4 mat, Vector4 vec)
        {
            TransformColumn(in mat, in vec, out Vector4 result);
            return result;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix using right-handed notation.
        /// </summary>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="result">The transformed vector.</param>
        public static void TransformColumn(in Matrix4 mat, in Vector4 vec, out Vector4 result)
        {
            result = new Vector4(
                (mat.Row0.X * vec.X) + (mat.Row0.Y * vec.Y) + (mat.Row0.Z * vec.Z) + (mat.Row0.W * vec.W),
                (mat.Row1.X * vec.X) + (mat.Row1.Y * vec.Y) + (mat.Row1.Z * vec.Z) + (mat.Row1.W * vec.W),
                (mat.Row2.X * vec.X) + (mat.Row2.Y * vec.Y) + (mat.Row2.Z * vec.Z) + (mat.Row2.W * vec.W),
                (mat.Row3.X * vec.X) + (mat.Row3.Y * vec.Y) + (mat.Row3.Z * vec.Z) + (mat.Row3.W * vec.W));
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2 with the X and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2 Xy
        {
            get => Unsafe.As<Vector4, Vector2>(ref this);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2 with the X and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2 Xz
        {
            readonly get => new(X, Z);
            set
            {
                X = value.X;
                Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2 with the X and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2 Xw
        {
            readonly get => new(X, W);
            set
            {
                X = value.X;
                W = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2 with the Y and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2 Yx
        {
            readonly get => new(Y, X);
            set
            {
                Y = value.X;
                X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2 with the Y and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2 Yz
        {
            readonly get => new(Y, Z);
            set
            {
                Y = value.X;
                Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2 with the Y and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2 Yw
        {
            readonly get => new(Y, W);
            set
            {
                Y = value.X;
                W = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2 with the Z and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2 Zx
        {
            readonly get => new(Z, X);
            set
            {
                Z = value.X;
                X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2 with the Z and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2 Zy
        {
            readonly get => new(Z, Y);
            set
            {
                Z = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2 with the Z and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2 Zw
        {
            readonly get => new(Z, W);
            set
            {
                Z = value.X;
                W = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2 with the W and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2 Wx
        {
            readonly get => new(W, X);
            set
            {
                W = value.X;
                X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2 with the W and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2 Wy
        {
            readonly get => new(W, Y);
            set
            {
                W = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2 with the W and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2 Wz
        {
            readonly get => new(W, Z);
            set
            {
                W = value.X;
                Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the X, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Xyz
        {
            get => Unsafe.As<Vector4, Vector3>(ref this);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the X, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Xyw
        {
            readonly get => new(X, Y, W);
            set
            {
                X = value.X;
                Y = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the X, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Xzy
        {
            readonly get => new(X, Z, Y);
            set
            {
                X = value.X;
                Z = value.Y;
                Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the X, Z, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Xzw
        {
            readonly get => new(X, Z, W);
            set
            {
                X = value.X;
                Z = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the X, W, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Xwy
        {
            readonly get => new(X, W, Y);
            set
            {
                X = value.X;
                W = value.Y;
                Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the X, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Xwz
        {
            readonly get => new(X, W, Z);
            set
            {
                X = value.X;
                W = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the Y, X, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Yxz
        {
            readonly get => new(Y, X, Z);
            set
            {
                Y = value.X;
                X = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the Y, X, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Yxw
        {
            readonly get => new(Y, X, W);
            set
            {
                Y = value.X;
                X = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the Y, Z, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Yzx
        {
            readonly get => new(Y, Z, X);
            set
            {
                Y = value.X;
                Z = value.Y;
                X = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the Y, Z, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Yzw
        {
            readonly get => new(Y, Z, W);
            set
            {
                Y = value.X;
                Z = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the Y, W, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Ywx
        {
            readonly get => new(Y, W, X);
            set
            {
                Y = value.X;
                W = value.Y;
                X = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the Y, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Ywz
        {
            readonly get => new(Y, W, Z);
            set
            {
                Y = value.X;
                W = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the Z, X, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Zxy
        {
            readonly get => new(Z, X, Y);
            set
            {
                Z = value.X;
                X = value.Y;
                Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the Z, X, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Zxw
        {
            readonly get => new(Z, X, W);
            set
            {
                Z = value.X;
                X = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the Z, Y, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Zyx
        {
            readonly get => new(Z, Y, X);
            set
            {
                Z = value.X;
                Y = value.Y;
                X = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the Z, Y, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Zyw
        {
            readonly get => new(Z, Y, W);
            set
            {
                Z = value.X;
                Y = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the Z, W, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Zwx
        {
            readonly get => new(Z, W, X);
            set
            {
                Z = value.X;
                W = value.Y;
                X = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the Z, W, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Zwy
        {
            readonly get => new(Z, W, Y);
            set
            {
                Z = value.X;
                W = value.Y;
                Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the W, X, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Wxy
        {
            readonly get => new(W, X, Y);
            set
            {
                W = value.X;
                X = value.Y;
                Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the W, X, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Wxz
        {
            readonly get => new(W, X, Z);
            set
            {
                W = value.X;
                X = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the W, Y, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Wyx
        {
            readonly get => new(W, Y, X);
            set
            {
                W = value.X;
                Y = value.Y;
                X = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the W, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Wyz
        {
            readonly get => new(W, Y, Z);
            set
            {
                W = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the W, Z, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Wzx
        {
            readonly get => new(W, Z, X);
            set
            {
                W = value.X;
                Z = value.Y;
                X = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3 with the W, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3 Wzy
        {
            readonly get => new(W, Z, Y);
            set
            {
                W = value.X;
                Z = value.Y;
                Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the X, Y, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Xywz
        {
            readonly get => new(X, Y, W, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                W = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the X, Z, Y, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Xzyw
        {
            readonly get => new(X, Z, Y, W);
            set
            {
                X = value.X;
                Z = value.Y;
                Y = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the X, Z, W, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Xzwy
        {
            readonly get => new(X, Z, W, Y);
            set
            {
                X = value.X;
                Z = value.Y;
                W = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the X, W, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Xwyz
        {
            readonly get => new(X, W, Y, Z);
            set
            {
                X = value.X;
                W = value.Y;
                Y = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the X, W, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Xwzy
        {
            readonly get => new(X, W, Z, Y);
            set
            {
                X = value.X;
                W = value.Y;
                Z = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Y, X, Z, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Yxzw
        {
            readonly get => new(Y, X, Z, W);
            set
            {
                Y = value.X;
                X = value.Y;
                Z = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Y, X, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Yxwz
        {
            readonly get => new(Y, X, W, Z);
            set
            {
                Y = value.X;
                X = value.Y;
                W = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Y, Y, Z, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Yyzw
        {
            readonly get => new(Y, Y, Z, W);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Y, Y, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Yywz
        {
            readonly get => new(Y, Y, W, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                W = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Y, Z, X, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Yzxw
        {
            readonly get => new(Y, Z, X, W);
            set
            {
                Y = value.X;
                Z = value.Y;
                X = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Y, Z, W, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Yzwx
        {
            readonly get => new(Y, Z, W, X);
            set
            {
                Y = value.X;
                Z = value.Y;
                W = value.Z;
                X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Y, W, X, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Ywxz
        {
            readonly get => new(Y, W, X, Z);
            set
            {
                Y = value.X;
                W = value.Y;
                X = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Y, W, Z, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Ywzx
        {
            readonly get => new(Y, W, Z, X);
            set
            {
                Y = value.X;
                W = value.Y;
                Z = value.Z;
                X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Z, X, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Zxyw
        {
            readonly get => new(Z, X, Y, W);
            set
            {
                Z = value.X;
                X = value.Y;
                Y = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Z, X, W, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Zxwy
        {
            readonly get => new(Z, X, W, Y);
            set
            {
                Z = value.X;
                X = value.Y;
                W = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Z, Y, X, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Zyxw
        {
            readonly get => new(Z, Y, X, W);
            set
            {
                Z = value.X;
                Y = value.Y;
                X = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Z, Y, W, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Zywx
        {
            readonly get => new(Z, Y, W, X);
            set
            {
                Z = value.X;
                Y = value.Y;
                W = value.Z;
                X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Z, W, X, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Zwxy
        {
            readonly get => new(Z, W, X, Y);
            set
            {
                Z = value.X;
                W = value.Y;
                X = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Z, W, Y, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Zwyx
        {
            readonly get => new(Z, W, Y, X);
            set
            {
                Z = value.X;
                W = value.Y;
                Y = value.Z;
                X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the Z, W, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Zwzy
        {
            readonly get => new(Z, W, Z, Y);
            set
            {
                X = value.X;
                W = value.Y;
                Z = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the W, X, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Wxyz
        {
            readonly get => new(W, X, Y, Z);
            set
            {
                W = value.X;
                X = value.Y;
                Y = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the W, X, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Wxzy
        {
            readonly get => new(W, X, Z, Y);
            set
            {
                W = value.X;
                X = value.Y;
                Z = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the W, Y, X, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Wyxz
        {
            readonly get => new(W, Y, X, Z);
            set
            {
                W = value.X;
                Y = value.Y;
                X = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the W, Y, Z, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Wyzx
        {
            readonly get => new(W, Y, Z, X);
            set
            {
                W = value.X;
                Y = value.Y;
                Z = value.Z;
                X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the W, Z, X, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Wzxy
        {
            readonly get => new(W, Z, X, Y);
            set
            {
                W = value.X;
                Z = value.Y;
                X = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the W, Z, Y, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Wzyx
        {
            readonly get => new(W, Z, Y, X);
            set
            {
                W = value.X;
                Z = value.Y;
                Y = value.Z;
                X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector4 with the W, Z, Y, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4 Wzyw
        {
            readonly get => new(W, Z, Y, W);
            set
            {
                X = value.X;
                Z = value.Y;
                Y = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Adds two instances.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static Vector4 operator +(Vector4 left, Vector4 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            left.W += right.W;
            return left;
        }

        /// <summary>
        /// Subtracts two instances.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static Vector4 operator -(Vector4 left, Vector4 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            left.W -= right.W;
            return left;
        }

        /// <summary>
        /// Negates an instance.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static Vector4 operator -(Vector4 vec)
        {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            vec.Z = -vec.Z;
            vec.W = -vec.W;
            return vec;
        }

        /// <summary>
        /// Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static Vector4 operator *(Vector4 vec, float scale)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        /// <summary>
        /// Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="scale">The scalar.</param>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static Vector4 operator *(float scale, Vector4 vec)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        /// <summary>
        /// Component-wise multiplication between the specified instance by a scale vector.
        /// </summary>
        /// <param name="scale">Left operand.</param>
        /// <param name="vec">Right operand.</param>
        /// <returns>Result of multiplication.</returns>
        [Pure]
        public static Vector4 operator *(Vector4 vec, Vector4 scale)
        {
            vec.X *= scale.X;
            vec.Y *= scale.Y;
            vec.Z *= scale.Z;
            vec.W *= scale.W;
            return vec;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="mat">The desired transformation.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector4 operator *(Vector4 vec, Matrix4 mat)
        {
            TransformRow(in vec, in mat, out Vector4 result);
            return result;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix using right-handed notation.
        /// </summary>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector4 operator *(Matrix4 mat, Vector4 vec)
        {
            TransformColumn(in mat, in vec, out Vector4 result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by a quaternion rotation.
        /// </summary>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector4 operator *(Quaternion quat, Vector4 vec)
        {
            Transform(in vec, in quat, out Vector4 result);
            return result;
        }

        /// <summary>
        /// Divides an instance by a scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static Vector4 operator /(Vector4 vec, float scale)
        {
            vec.X /= scale;
            vec.Y /= scale;
            vec.Z /= scale;
            vec.W /= scale;
            return vec;
        }

        /// <summary>
        /// Component-wise division between the specified instance by a scale vector.
        /// </summary>
        /// <param name="vec">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the division.</returns>
        [Pure]
        public static Vector4 operator /(Vector4 vec, Vector4 scale)
        {
            vec.X /= scale.X;
            vec.Y /= scale.Y;
            vec.Z /= scale.Z;
            vec.W /= scale.W;
            return vec;
        }

        /// <summary>
        /// Compares two instances for equality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        public static bool operator ==(Vector4 left, Vector4 right) => left.Equals(right);

        /// <summary>
        /// Compares two instances for inequality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equa lright; false otherwise.</returns>
        public static bool operator !=(Vector4 left, Vector4 right) => !(left == right);

        /// <summary>
        /// Returns a pointer to the first element of the specified instance.
        /// </summary>
        /// <param name="v">The instance.</param>
        /// <returns>A pointer to the first element of v.</returns>
        [Pure]
        public static unsafe explicit operator float*(Vector4 v) => &v.X;

        /// <summary>
        /// Returns a pointer to the first element of the specified instance.
        /// </summary>
        /// <param name="v">The instance.</param>
        /// <returns>A pointer to the first element of v.</returns>
        [Pure]
        public static explicit operator IntPtr(Vector4 v)
        {
            unsafe
            {
                return (IntPtr)(&v.X);
            }
        }

        /// <summary>
        /// Returns this Vector4 as a Color4. The resulting struct will have RGBA mapped to XYZW, in that order.
        /// </summary>
        /// <param name="v">The Vector4 to convert.</param>
        /// <returns>The Vector4, converted to a Color4.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static explicit operator Color4(Vector4 v) => Unsafe.As<Vector4, Color4>(ref v);

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector4 to SatisfactorySaveNet.Vector4d.
        /// </summary>
        /// <param name="vec">The Vector4 to convert.</param>
        /// <returns>The resulting Vector4d.</returns>
        [Pure]
        public static implicit operator Vector4D(Vector4 vec) => new(vec.X, vec.Y, vec.Z, vec.W);

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector4 to SatisfactorySaveNet.Vector4h.
        /// </summary>
        /// <param name="vec">The Vector4 to convert.</param>
        /// <returns>The resulting Vector4h.</returns>
        [Pure]
        public static explicit operator Vector4H(Vector4 vec) => new(vec.X, vec.Y, vec.Z, vec.W);

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector4 to SatisfactorySaveNet.Vector4i.
        /// </summary>
        /// <param name="vec">The Vector4 to convert.</param>
        /// <returns>The resulting Vector4i.</returns>
        [Pure]
        public static explicit operator Vector4I(Vector4 vec) => new((int)vec.X, (int)vec.Y, (int)vec.Z, (int)vec.W);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> struct using a tuple containing the component
        /// values.
        /// </summary>
        /// <param name="values">A tuple containing the component values.</param>
        /// <returns>A new instance of the <see cref="Vector4"/> struct with the given component values.</returns>
        [Pure]
        public static implicit operator Vector4((float X, float Y, float Z, float W) values) => new(values.X, values.Y, values.Z, values.W);

        /// <inheritdoc/>
        public override readonly string ToString() => ToString(null, null);

        /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
        public readonly string ToString(string format) => ToString(format, null);

        /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
        public readonly string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);

        /// <inheritdoc />
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            return string.Format(
                "({0}{4} {1}{4} {2}{4} {3})",
                X.ToString(format, formatProvider),
                Y.ToString(format, formatProvider),
                Z.ToString(format, formatProvider),
                W.ToString(format, formatProvider),
                MathHelper.GetListSeparator(formatProvider));
        }

        /// <inheritdoc />
        public override readonly bool Equals(object? obj) => obj is Vector4 vector && Equals(vector);

        /// <inheritdoc />
        public readonly bool Equals(Vector4 other) => X == other.X &&
                   Y == other.Y &&
                   Z == other.Z &&
                   W == other.W;

        /// <inheritdoc />
#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z, W);
#pragma warning restore S2328 // "GetHashCode" should not reference mutable fields

        /// <summary>
        /// Deconstructs the vector into it's individual components.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        /// <param name="w">The W component of the vector.</param>
        [Pure]
        public readonly void Deconstruct(out float x, out float y, out float z, out float w)
        {
            x = X;
            y = Y;
            z = Z;
            w = W;
        }
    }
}
