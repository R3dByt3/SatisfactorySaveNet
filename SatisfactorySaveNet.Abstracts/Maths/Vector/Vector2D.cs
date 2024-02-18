using SatisfactorySaveNet.Abstracts.Maths.Data;
using SatisfactorySaveNet.Abstracts.Maths.Matrix;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Half = SatisfactorySaveNet.Abstracts.Maths.Data.Half;

namespace SatisfactorySaveNet.Abstracts.Maths.Vector
{
    /// <summary>
    /// Represents a 2D vector using two double-precision floating-point numbers.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2D : IEquatable<Vector2D>, IFormattable
    {
        /// <summary>
        /// The X coordinate of this instance.
        /// </summary>
        public double X;

        /// <summary>
        /// The Y coordinate of this instance.
        /// </summary>
        public double Y;

        /// <summary>
        /// Defines a unit-length Vector2d that points towards the X-axis.
        /// </summary>
        public static readonly Vector2D UnitX = new(1, 0);

        /// <summary>
        /// Defines a unit-length Vector2d that points towards the Y-axis.
        /// </summary>
        public static readonly Vector2D UnitY = new(0, 1);

        /// <summary>
        /// Defines an instance with all components set to 0.
        /// </summary>
        public static readonly Vector2D Zero = new(0, 0);

        /// <summary>
        /// Defines an instance with all components set to 1.
        /// </summary>
        public static readonly Vector2D One = new(1, 1);

        /// <summary>
        /// Defines an instance with all components set to positive infinity.
        /// </summary>
        public static readonly Vector2D PositiveInfinity = new(double.PositiveInfinity, double.PositiveInfinity);

        /// <summary>
        /// Defines an instance with all components set to negative infinity.
        /// </summary>
        public static readonly Vector2D NegativeInfinity = new(double.NegativeInfinity, double.NegativeInfinity);

        /// <summary>
        /// Defines the size of the Vector2d struct in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Unsafe.SizeOf<Vector2>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2D"/> struct.
        /// </summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public Vector2D(double value)
        {
            X = value;
            Y = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2D"/> struct.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets or sets the value at the index of the Vector.
        /// </summary>
        /// <param name="index">The index of the component from the Vector.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index is less than 0 or greater than 1.</exception>
        public double this[int index]
        {
            readonly get
            {
                var tmp = index == 0 ? X : index;
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                return tmp == 1 ? Y : throw new IndexOutOfRangeException("You tried to access this vector at index: " + index);
#pragma warning restore S112 // General or reserved exceptions should never be thrown
            }
            set
            {
                if (index == 0)
                {
                    X = value;
                }
                else
                {
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                    Y = index == 1 ? value : throw new IndexOutOfRangeException("You tried to set this vector at index: " + index);
#pragma warning restore S112 // General or reserved exceptions should never be thrown
                }
            }
        }

        /// <summary>
        /// Gets the length (magnitude) of the vector.
        /// </summary>
        /// <seealso cref="LengthSquared"/>
        public readonly double Length => Math.Sqrt((X * X) + (Y * Y));

        /// <summary>
        /// Gets the square of the vector length (magnitude).
        /// </summary>
        /// <remarks>
        /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
        /// for comparisons.
        /// </remarks>
        /// <see cref="Length"/>
        public readonly double LengthSquared => (X * X) + (Y * Y);

        /// <summary>
        /// Gets the perpendicular vector on the right side of this vector.
        /// </summary>
        public readonly Vector2D PerpendicularRight => new(Y, -X);

        /// <summary>
        /// Gets the perpendicular vector on the left side of this vector.
        /// </summary>
        public readonly Vector2D PerpendicularLeft => new(-Y, X);

        /// <summary>
        /// Returns a copy of the Vector2d scaled to unit length.
        /// </summary>
        /// <returns>The normalized copy.</returns>
        public readonly Vector2D Normalized()
        {
            Vector2D v = this;
            v.Normalize();
            return v;
        }

        /// <summary>
        /// Scales the Vector2 to unit length.
        /// </summary>
        public void Normalize()
        {
            double scale = 1.0 / Length;
            X *= scale;
            Y *= scale;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Result of operation.</returns>
        [Pure]
        public static Vector2D Add(Vector2D a, Vector2D b)
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
        public static void Add(in Vector2D a, in Vector2D b, out Vector2D result)
        {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
        }

        /// <summary>
        /// Subtract one Vector from another.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>Result of subtraction.</returns>
        [Pure]
        public static Vector2D Subtract(Vector2D a, Vector2D b)
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
        public static void Subtract(in Vector2D a, in Vector2D b, out Vector2D result)
        {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static Vector2D Multiply(Vector2D vector, double scale)
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
        public static void Multiply(in Vector2D vector, double scale, out Vector2D result)
        {
            result.X = vector.X * scale;
            result.Y = vector.Y * scale;
        }

        /// <summary>
        /// Multiplies a vector by the components a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static Vector2D Multiply(Vector2D vector, Vector2D scale)
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
        public static void Multiply(in Vector2D vector, in Vector2D scale, out Vector2D result)
        {
            result.X = vector.X * scale.X;
            result.Y = vector.Y * scale.Y;
        }

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static Vector2D Divide(Vector2D vector, double scale)
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
        public static void Divide(in Vector2D vector, double scale, out Vector2D result)
        {
            result.X = vector.X / scale;
            result.Y = vector.Y / scale;
        }

        /// <summary>
        /// Divides a vector by the components of a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static Vector2D Divide(Vector2D vector, Vector2D scale)
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
        public static void Divide(in Vector2D vector, in Vector2D scale, out Vector2D result)
        {
            result.X = vector.X / scale.X;
            result.Y = vector.Y / scale.Y;
        }

        /// <summary>
        /// Returns a vector created from the smallest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>The component-wise minimum.</returns>
        [Pure]
        public static Vector2D ComponentMin(Vector2D a, Vector2D b)
        {
            a.X = a.X < b.X ? a.X : b.X;
            a.Y = a.Y < b.Y ? a.Y : b.Y;
            return a;
        }

        /// <summary>
        /// Returns a vector created from the smallest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">The component-wise minimum.</param>
        public static void ComponentMin(in Vector2D a, in Vector2D b, out Vector2D result)
        {
            result.X = a.X < b.X ? a.X : b.X;
            result.Y = a.Y < b.Y ? a.Y : b.Y;
        }

        /// <summary>
        /// Returns a vector created from the largest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>The component-wise maximum.</returns>
        [Pure]
        public static Vector2D ComponentMax(Vector2D a, Vector2D b)
        {
            a.X = a.X > b.X ? a.X : b.X;
            a.Y = a.Y > b.Y ? a.Y : b.Y;
            return a;
        }

        /// <summary>
        /// Returns a vector created from the largest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">The component-wise maximum.</param>
        public static void ComponentMax(in Vector2D a, in Vector2D b, out Vector2D result)
        {
            result.X = a.X > b.X ? a.X : b.X;
            result.Y = a.Y > b.Y ? a.Y : b.Y;
        }

        /// <summary>
        /// Returns the Vector2d with the minimum magnitude. If the magnitudes are equal, the second vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>The minimum Vector2d.</returns>
        [Pure]
        public static Vector2D MagnitudeMin(Vector2D left, Vector2D right) => left.LengthSquared < right.LengthSquared ? left : right;

        /// <summary>
        /// Returns the Vector2d with the minimum magnitude. If the magnitudes are equal, the second vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <param name="result">The magnitude-wise minimum.</param>
        public static void MagnitudeMin(in Vector2D left, in Vector2D right, out Vector2D result)
        {
            result = left.LengthSquared < right.LengthSquared ? left : right;
        }

        /// <summary>
        /// Returns the Vector2d with the minimum magnitude. If the magnitudes are equal, the first vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>The minimum Vector2d.</returns>
        [Pure]
        public static Vector2D MagnitudeMax(Vector2D left, Vector2D right) => left.LengthSquared >= right.LengthSquared ? left : right;

        /// <summary>
        /// Returns the Vector2d with the maximum magnitude. If the magnitudes are equal, the first vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <param name="result">The magnitude-wise maximum.</param>
        public static void MagnitudeMax(in Vector2D left, in Vector2D right, out Vector2D result)
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
        public static Vector2D Clamp(Vector2D vec, Vector2D min, Vector2D max)
        {
            var tmpX = vec.X < min.X ? min.X : vec.X;
            vec.X = tmpX > max.X ? max.X : vec.X;
            var tmpY = vec.Y < min.Y ? min.Y : vec.Y;
            vec.Y = tmpY > max.Y ? max.Y : vec.Y;
            return vec;
        }

        /// <summary>
        /// Clamp a vector to the given minimum and maximum vectors.
        /// </summary>
        /// <param name="vec">Input vector.</param>
        /// <param name="min">Minimum vector.</param>
        /// <param name="max">Maximum vector.</param>
        /// <param name="result">The clamped vector.</param>
        public static void Clamp(in Vector2D vec, in Vector2D min, in Vector2D max, out Vector2D result)
        {
            var tmpX = vec.X < min.X ? min.X : vec.X;
            result.X = tmpX > max.X ? max.X : vec.X;
            var tmpY = vec.Y < min.Y ? min.Y : vec.Y;
            result.Y = tmpY > max.Y ? max.Y : vec.Y;
        }

        /// <summary>
        /// Compute the euclidean distance between two vectors.
        /// </summary>
        /// <param name="vec1">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>The distance.</returns>
        [Pure]
        public static double Distance(Vector2D vec1, Vector2D vec2)
        {
            Distance(in vec1, in vec2, out double result);
            return result;
        }

        /// <summary>
        /// Compute the euclidean distance between two vectors.
        /// </summary>
        /// <param name="vec1">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <param name="result">The distance.</param>
        public static void Distance(in Vector2D vec1, in Vector2D vec2, out double result)
        {
            result = Math.Sqrt(((vec2.X - vec1.X) * (vec2.X - vec1.X)) + ((vec2.Y - vec1.Y) * (vec2.Y - vec1.Y)));
        }

        /// <summary>
        /// Compute the squared euclidean distance between two vectors.
        /// </summary>
        /// <param name="vec1">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>The squared distance.</returns>
        [Pure]
        public static double DistanceSquared(Vector2D vec1, Vector2D vec2)
        {
            DistanceSquared(in vec1, in vec2, out double result);
            return result;
        }

        /// <summary>
        /// Compute the squared euclidean distance between two vectors.
        /// </summary>
        /// <param name="vec1">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <param name="result">The squared distance.</param>
        public static void DistanceSquared(in Vector2D vec1, in Vector2D vec2, out double result)
        {
            result = ((vec2.X - vec1.X) * (vec2.X - vec1.X)) + ((vec2.Y - vec1.Y) * (vec2.Y - vec1.Y));
        }

        /// <summary>
        /// Scale a vector to unit length.
        /// </summary>
        /// <param name="vec">The input vector.</param>
        /// <returns>The normalized copy.</returns>
        [Pure]
        public static Vector2D Normalize(Vector2D vec)
        {
            double scale = 1.0 / vec.Length;
            vec.X *= scale;
            vec.Y *= scale;
            return vec;
        }

        /// <summary>
        /// Scale a vector to unit length.
        /// </summary>
        /// <param name="vec">The input vector.</param>
        /// <param name="result">The normalized vector.</param>
        public static void Normalize(in Vector2D vec, out Vector2D result)
        {
            double scale = 1.0 / vec.Length;
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
        }

        /// <summary>
        /// Scale a vector to approximately unit length.
        /// </summary>
        /// <param name="vec">The input vector.</param>
        /// <returns>The normalized copy.</returns>
        [Pure]
        public static Vector2D NormalizeFast(Vector2D vec)
        {
            double scale = MathHelper.InverseSqrtFast((vec.X * vec.X) + (vec.Y * vec.Y));
            vec.X *= scale;
            vec.Y *= scale;
            return vec;
        }

        /// <summary>
        /// Scale a vector to approximately unit length.
        /// </summary>
        /// <param name="vec">The input vector.</param>
        /// <param name="result">The normalized vector.</param>
        public static void NormalizeFast(in Vector2D vec, out Vector2D result)
        {
            double scale = MathHelper.InverseSqrtFast((vec.X * vec.X) + (vec.Y * vec.Y));
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
        }

        /// <summary>
        /// Calculate the dot (scalar) product of two vectors.
        /// </summary>
        /// <param name="left">First operand.</param>
        /// <param name="right">Second operand.</param>
        /// <returns>The dot product of the two inputs.</returns>
        [Pure]
        public static double Dot(Vector2D left, Vector2D right) => (left.X * right.X) + (left.Y * right.Y);

        /// <summary>
        /// Calculate the dot (scalar) product of two vectors.
        /// </summary>
        /// <param name="left">First operand.</param>
        /// <param name="right">Second operand.</param>
        /// <param name="result">The dot product of the two inputs.</param>
        public static void Dot(in Vector2D left, in Vector2D right, out double result)
        {
            result = (left.X * right.X) + (left.Y * right.Y);
        }

        /// <summary>
        /// Returns a new vector that is the linear blend of the 2 given vectors.
        /// </summary>
        /// <param name="a">First input vector.</param>
        /// <param name="b">Second input vector.</param>
        /// <param name="blend">The blend factor.</param>
        /// <returns>a when blend=0, b when blend=1, and a linear combination otherwise.</returns>
        [Pure]
        public static Vector2D Lerp(Vector2D a, Vector2D b, double blend)
        {
            a.X = (blend * (b.X - a.X)) + a.X;
            a.Y = (blend * (b.Y - a.Y)) + a.Y;
            return a;
        }

        /// <summary>
        /// Returns a new vector that is the linear blend of the 2 given vectors.
        /// </summary>
        /// <param name="a">First input vector.</param>
        /// <param name="b">Second input vector.</param>
        /// <param name="blend">The blend factor.</param>
        /// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise.</param>
        public static void Lerp(in Vector2D a, in Vector2D b, double blend, out Vector2D result)
        {
            result.X = (blend * (b.X - a.X)) + a.X;
            result.Y = (blend * (b.Y - a.Y)) + a.Y;
        }

        /// <summary>
        /// Returns a new vector that is the component-wise linear blend of the 2 given vectors.
        /// </summary>
        /// <param name="a">First input vector.</param>
        /// <param name="b">Second input vector.</param>
        /// <param name="blend">The blend factor.</param>
        /// <returns>a when blend=0, b when blend=1, and a component-wise linear combination otherwise.</returns>
        [Pure]
        public static Vector2D Lerp(Vector2D a, Vector2D b, Vector2D blend)
        {
            a.X = (blend.X * (b.X - a.X)) + a.X;
            a.Y = (blend.Y * (b.Y - a.Y)) + a.Y;
            return a;
        }

        /// <summary>
        /// Returns a new vector that is the component-wise linear blend of the 2 given vectors.
        /// </summary>
        /// <param name="a">First input vector.</param>
        /// <param name="b">Second input vector.</param>
        /// <param name="blend">The blend factor.</param>
        /// <param name="result">a when blend=0, b when blend=1, and a component-wise linear combination otherwise.</param>
        public static void Lerp(in Vector2D a, in Vector2D b, Vector2D blend, out Vector2D result)
        {
            result.X = (blend.X * (b.X - a.X)) + a.X;
            result.Y = (blend.Y * (b.Y - a.Y)) + a.Y;
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
        public static Vector2D BaryCentric(Vector2D a, Vector2D b, Vector2D c, double u, double v)
        {
            BaryCentric(in a, in b, in c, u, v, out Vector2D result);
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
            in Vector2D a,
            in Vector2D b,
            in Vector2D c,
            double u,
            double v,
            out Vector2D result
        )
        {
#pragma warning disable S2234 // Arguments should be passed in the same order as the method parameters
            Subtract(in b, in a, out Vector2D ab);
#pragma warning restore S2234 // Arguments should be passed in the same order as the method parameters
            Multiply(in ab, u, out Vector2D abU);
            Add(in a, in abU, out Vector2D uPos);

            Subtract(in c, in a, out Vector2D ac);
            Multiply(in ac, v, out Vector2D acV);
            Add(in uPos, in acV, out result);
        }

        /// <summary>
        /// Transform a Vector by the given Matrix.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="mat">The desired transformation.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector2D TransformRow(Vector2D vec, Matrix2D mat)
        {
            TransformRow(in vec, in mat, out Vector2D result);
            return result;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="result">The transformed vector.</param>
        public static void TransformRow(in Vector2D vec, in Matrix2D mat, out Vector2D result)
        {
            result = new Vector2D(
                (vec.X * mat.Row0.X) + (vec.Y * mat.Row1.X),
                (vec.X * mat.Row0.Y) + (vec.Y * mat.Row1.Y));
        }

        /// <summary>
        /// Transforms a vector by a quaternion rotation.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <returns>The result of the operation.</returns>
        [Pure]
        public static Vector2D Transform(Vector2D vec, QuaternionD quat)
        {
            Transform(in vec, in quat, out Vector2D result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by a quaternion rotation.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <param name="result">The result of the operation.</param>
        public static void Transform(in Vector2D vec, in QuaternionD quat, out Vector2D result)
        {
            QuaternionD v = new(vec.X, vec.Y, 0, 0);
            QuaternionD.Invert(in quat, out QuaternionD i);
            QuaternionD.Multiply(in quat, in v, out QuaternionD t);
            QuaternionD.Multiply(in t, in i, out v);

            result.X = v.X;
            result.Y = v.Y;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix using right-handed notation.
        /// </summary>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector2D TransformColumn(Matrix2D mat, Vector2D vec)
        {
            TransformColumn(in mat, in vec, out Vector2D result);
            return result;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix using right-handed notation.
        /// </summary>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="result">The transformed vector.</param>
        public static void TransformColumn(in Matrix2D mat, in Vector2D vec, out Vector2D result)
        {
            result.X = (mat.Row0.X * vec.X) + (mat.Row0.Y * vec.Y);
            result.Y = (mat.Row1.X * vec.X) + (mat.Row1.Y * vec.Y);
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2d with the Y and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2D Yx
        {
            readonly get => new(Y, X);
            set
            {
                Y = value.X;
                X = value.Y;
            }
        }

        /// <summary>
        /// Adds two instances.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns>The result of the operation.</returns>
        [Pure]
        public static Vector2D operator +(Vector2D left, Vector2D right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }

        /// <summary>
        /// Subtracts two instances.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns>The result of the operation.</returns>
        [Pure]
        public static Vector2D operator -(Vector2D left, Vector2D right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        /// <summary>
        /// Negates an instance.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the operation.</returns>
        [Pure]
        public static Vector2D operator -(Vector2D vec)
        {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            return vec;
        }

        /// <summary>
        /// Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="f">The scalar.</param>
        /// <returns>The result of the operation.</returns>
        [Pure]
        public static Vector2D operator *(Vector2D vec, double f)
        {
            vec.X *= f;
            vec.Y *= f;
            return vec;
        }

        /// <summary>
        /// Multiply an instance by a scalar.
        /// </summary>
        /// <param name="f">The scalar.</param>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the operation.</returns>
        [Pure]
        public static Vector2D operator *(double f, Vector2D vec)
        {
            vec.X *= f;
            vec.Y *= f;
            return vec;
        }

        /// <summary>
        /// Component-wise multiplication between the specified instance by a scale vector.
        /// </summary>
        /// <param name="scale">Left operand.</param>
        /// <param name="vec">Right operand.</param>
        /// <returns>Result of multiplication.</returns>
        [Pure]
        public static Vector2D operator *(Vector2D vec, Vector2D scale)
        {
            vec.X *= scale.X;
            vec.Y *= scale.Y;
            return vec;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="mat">The desired transformation.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector2D operator *(Vector2D vec, Matrix2D mat)
        {
            TransformRow(in vec, in mat, out Vector2D result);
            return result;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix using right-handed notation.
        /// </summary>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector2D operator *(Matrix2D mat, Vector2D vec)
        {
            TransformColumn(in mat, in vec, out Vector2D result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by a quaternion rotation.
        /// </summary>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static Vector2D operator *(QuaternionD quat, Vector2D vec)
        {
            Transform(in vec, in quat, out Vector2D result);
            return result;
        }

        /// <summary>
        /// Divides an instance by a scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="f">The scalar.</param>
        /// <returns>The result of the operation.</returns>
        [Pure]
        public static Vector2D operator /(Vector2D vec, double f)
        {
            vec.X /= f;
            vec.Y /= f;
            return vec;
        }

        /// <summary>
        /// Component-wise division between the specified instance by a scale vector.
        /// </summary>
        /// <param name="vec">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the division.</returns>
        [Pure]
        public static Vector2D operator /(Vector2D vec, Vector2D scale)
        {
            vec.X /= scale.X;
            vec.Y /= scale.Y;
            return vec;
        }

        /// <summary>
        /// Compares two instances for equality.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns>True, if both instances are equal; false otherwise.</returns>
        public static bool operator ==(Vector2D left, Vector2D right) => left.Equals(right);

        /// <summary>
        /// Compares two instances for ienquality.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns>True, if the instances are not equal; false otherwise.</returns>
        public static bool operator !=(Vector2D left, Vector2D right) => !(left == right);

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector2d to SatisfactorySaveNet.Vector2.
        /// </summary>
        /// <param name="vec">The Vector2d to convert.</param>
        /// <returns>The resulting Vector2.</returns>
        [Pure]
        public static explicit operator Vector2(Vector2D vec) => new((float)vec.X, (float)vec.Y);

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector2d to SatisfactorySaveNet.Vector2h.
        /// </summary>
        /// <param name="vec">The Vector2d to convert.</param>
        /// <returns>The resulting Vector2h.</returns>
        [Pure]
        public static explicit operator Vector2H(Vector2D vec) => new(new Half(vec.X), new Half(vec.Y));

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector2d to SatisfactorySaveNet.Vector2i.
        /// </summary>
        /// <param name="vec">The Vector2d to convert.</param>
        /// <returns>The resulting Vector2i.</returns>
        [Pure]
        public static explicit operator Vector2I(Vector2D vec) => new((int)vec.X, (int)vec.Y);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2D"/> struct using a tuple containing the component
        /// values.
        /// </summary>
        /// <param name="values">A tuple containing the component values.</param>
        /// <returns>A new instance of the <see cref="Vector2D"/> struct with the given component values.</returns>
        [Pure]
        public static implicit operator Vector2D((double X, double Y) values) => new(values.X, values.Y);

        /// <inheritdoc/>
        public override readonly string ToString() => ToString(null, null);

        /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
        public readonly string ToString(string format) => ToString(format, null);

        /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
        public readonly string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);

        /// <inheritdoc/>
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            return string.Format(
                "({0}{2} {1})",
                X.ToString(format, formatProvider),
                Y.ToString(format, formatProvider),
                MathHelper.GetListSeparator(formatProvider));
        }

        /// <inheritdoc/>
        public override readonly bool Equals(object? obj) => obj is Vector2D vector && Equals(vector);

        /// <inheritdoc/>
        public readonly bool Equals(Vector2D other) => X == other.X &&
                   Y == other.Y;

        /// <inheritdoc/>
#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
        public override readonly int GetHashCode() => HashCode.Combine(X, Y);
#pragma warning restore S2328 // "GetHashCode" should not reference mutable fields

        /// <summary>
        /// Deconstructs the vector into it's individual components.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        [Pure]
        public readonly void Deconstruct(out double x, out double y)
        {
            x = X;
            y = Y;
        }
    }
}
