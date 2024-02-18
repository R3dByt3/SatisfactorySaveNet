using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace SatisfactorySaveNet.Abstracts.Maths.Vector
{
    /// <summary>
    /// Represents a 3D vector using three 32-bit integer numbers.
    /// </summary>
    /// <remarks>
    /// The Vector3i structure is suitable for interoperation with unmanaged code requiring three consecutive integers.
    /// </remarks>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3I : IEquatable<Vector3I>, IFormattable
    {
        /// <summary>
        /// The X component of the Vector3i.
        /// </summary>
        public int X;

        /// <summary>
        /// The Y component of the Vector3i.
        /// </summary>
        public int Y;

        /// <summary>
        /// The Z component of the Vector3i.
        /// </summary>
        public int Z;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3I"/> struct.
        /// </summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public Vector3I(int value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3I"/> struct.
        /// </summary>
        /// <param name="x">The x component of the Vector3.</param>
        /// <param name="y">The y component of the Vector3.</param>
        /// <param name="z">The z component of the Vector3.</param>
        public Vector3I(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3I"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector2I"/> to copy components from.</param>
        public Vector3I(Vector2I v)
        {
            X = v.X;
            Y = v.Y;
            Z = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3I"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector2I"/> to copy components from.</param>
        /// <param name="z">The z component of the new Vector3.</param>
        public Vector3I(Vector2I v, int z)
        {
            X = v.X;
            Y = v.Y;
            Z = z;
        }

        /// <summary>
        /// Gets or sets the value at the index of the vector.
        /// </summary>
        /// <param name="index">The index of the component from the vector.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index is less than 0 or greater than 2.</exception>
        public int this[int index]
        {
            readonly get
            {
                if (index == 0)
                {
                    return X;
                }

                var tmp = index == 1 ? Y : index;

#pragma warning disable S112 // General or reserved exceptions should never be thrown
                return tmp == 2 ? Z : throw new IndexOutOfRangeException("You tried to access this vector at index: " + index);
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
                else
                {
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                    Z = index == 2 ? value : throw new IndexOutOfRangeException("You tried to set this vector at index: " + index);
#pragma warning restore S112 // General or reserved exceptions should never be thrown
                }
            }
        }

        /// <summary>
        /// Gets the manhattan length of the vector.
        /// </summary>
        public readonly int ManhattanLength => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);

        /// <summary>
        /// Gets the squared euclidean length of the vector.
        /// </summary>
        public readonly int EuclideanLengthSquared => (X * X) + (Y * Y) + (Z * Z);

        /// <summary>
        /// Gets the euclidean length of the vector.
        /// </summary>
        public readonly float EuclideanLength => MathF.Sqrt((X * X) + (Y * Y) + (Z * Z));

        /// <summary>
        /// Defines a unit-length Vector3i that points towards the X-axis.
        /// </summary>
        public static readonly Vector3I UnitX = new(1, 0, 0);

        /// <summary>
        /// Defines a unit-length Vector3i that points towards the Y-axis.
        /// </summary>
        public static readonly Vector3I UnitY = new(0, 1, 0);

        /// <summary>
        /// Defines a unit-length Vector3i that points towards the Z-axis.
        /// </summary>
        public static readonly Vector3I UnitZ = new(0, 0, 1);

        /// <summary>
        /// Defines an instance with all components set to 0.
        /// </summary>
        public static readonly Vector3I Zero = new(0, 0, 0);

        /// <summary>
        /// Defines an instance with all components set to 1.
        /// </summary>
        public static readonly Vector3I One = new(1, 1, 1);

        /// <summary>
        /// Defines the size of the Vector3i struct in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Unsafe.SizeOf<Vector3I>();

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Result of operation.</returns>
        [Pure]
        public static Vector3I Add(Vector3I a, Vector3I b)
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
        public static void Add(in Vector3I a, in Vector3I b, out Vector3I result)
        {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
            result.Z = a.Z + b.Z;
        }

        /// <summary>
        /// Subtract one vector from another.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>Result of subtraction.</returns>
        [Pure]
        public static Vector3I Subtract(Vector3I a, Vector3I b)
        {
            Subtract(in a, in b, out a);
            return a;
        }

        /// <summary>
        /// Subtract one vector from another.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Result of subtraction.</param>
        public static void Subtract(in Vector3I a, in Vector3I b, out Vector3I result)
        {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
            result.Z = a.Z - b.Z;
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static Vector3I Multiply(Vector3I vector, int scale)
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
        public static void Multiply(in Vector3I vector, int scale, out Vector3I result)
        {
            result.X = vector.X * scale;
            result.Y = vector.Y * scale;
            result.Z = vector.Z * scale;
        }

        /// <summary>
        /// Multiplies a vector by the components a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static Vector3I Multiply(Vector3I vector, Vector3I scale)
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
        public static void Multiply(in Vector3I vector, in Vector3I scale, out Vector3I result)
        {
            result.X = vector.X * scale.X;
            result.Y = vector.Y * scale.Y;
            result.Z = vector.Z * scale.Z;
        }

        /// <summary>
        /// Divides a vector by a scalar using integer division, floor(a/b).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static Vector3I Divide(Vector3I vector, int scale)
        {
            Divide(in vector, scale, out vector);
            return vector;
        }

        /// <summary>
        /// Divides a vector by a scalar using integer division, floor(a/b).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Divide(in Vector3I vector, int scale, out Vector3I result)
        {
            result.X = vector.X / scale;
            result.Y = vector.Y / scale;
            result.Z = vector.Z / scale;
        }

        /// <summary>
        /// Divides a vector by the components of a vector using integer division, floor(a/b).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static Vector3I Divide(Vector3I vector, Vector3I scale)
        {
            Divide(in vector, in scale, out vector);
            return vector;
        }

        /// <summary>
        /// Divides a vector by the components of a vector using integer division, floor(a/b).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Divide(in Vector3I vector, in Vector3I scale, out Vector3I result)
        {
            result.X = vector.X / scale.X;
            result.Y = vector.Y / scale.Y;
            result.Z = vector.Z / scale.Z;
        }

        /// <summary>
        /// Returns a vector created from the smallest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>The component-wise minimum.</returns>
        [Pure]
        public static Vector3I ComponentMin(Vector3I a, Vector3I b)
        {
            Vector3I result;
            result.X = Math.Min(a.X, b.X);
            result.Y = Math.Min(a.Y, b.Y);
            result.Z = Math.Min(a.Z, b.Z);
            return result;
        }

        /// <summary>
        /// Returns a vector created from the smallest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">The component-wise minimum.</param>
        public static void ComponentMin(in Vector3I a, in Vector3I b, out Vector3I result)
        {
            result.X = Math.Min(a.X, b.X);
            result.Y = Math.Min(a.Y, b.Y);
            result.Z = Math.Min(a.Z, b.Z);
        }

        /// <summary>
        /// Returns a vector created from the largest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>The component-wise maximum.</returns>
        [Pure]
        public static Vector3I ComponentMax(Vector3I a, Vector3I b)
        {
            Vector3I result;
            result.X = Math.Max(a.X, b.X);
            result.Y = Math.Max(a.Y, b.Y);
            result.Z = Math.Max(a.Z, b.Z);
            return result;
        }

        /// <summary>
        /// Returns a vector created from the largest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">The component-wise maximum.</param>
        public static void ComponentMax(in Vector3I a, in Vector3I b, out Vector3I result)
        {
            result.X = Math.Max(a.X, b.X);
            result.Y = Math.Max(a.Y, b.Y);
            result.Z = Math.Max(a.Z, b.Z);
        }

        /// <summary>
        /// Clamp a vector to the given minimum and maximum vectors.
        /// </summary>
        /// <param name="vec">Input vector.</param>
        /// <param name="min">Minimum vector.</param>
        /// <param name="max">Maximum vector.</param>
        /// <returns>The clamped vector.</returns>
        [Pure]
        public static Vector3I Clamp(Vector3I vec, Vector3I min, Vector3I max)
        {
            Vector3I result;
            result.X = MathHelper.Clamp(vec.X, min.X, max.X);
            result.Y = MathHelper.Clamp(vec.Y, min.Y, max.Y);
            result.Z = MathHelper.Clamp(vec.Z, min.Z, max.Z);
            return result;
        }

        /// <summary>
        /// Clamp a vector to the given minimum and maximum vectors.
        /// </summary>
        /// <param name="vec">Input vector.</param>
        /// <param name="min">Minimum vector.</param>
        /// <param name="max">Maximum vector.</param>
        /// <param name="result">The clamped vector.</param>
        public static void Clamp(in Vector3I vec, in Vector3I min, in Vector3I max, out Vector3I result)
        {
            result.X = MathHelper.Clamp(vec.X, min.X, max.X);
            result.Y = MathHelper.Clamp(vec.Y, min.Y, max.Y);
            result.Z = MathHelper.Clamp(vec.Z, min.Z, max.Z);
        }

        /// <summary>
        /// Gets or sets a <see cref="Vector2I"/> with the X and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2I Xy
        {
            get => Unsafe.As<Vector3I, Vector2I>(ref this);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="Vector2I"/> with the X and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2I Xz
        {
            readonly get => new(X, Z);
            set
            {
                X = value.X;
                Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="Vector2I"/> with the Y and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2I Yx
        {
            readonly get => new(Y, X);
            set
            {
                Y = value.X;
                X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="Vector2I"/> with the Y and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2I Yz
        {
            readonly get => new(Y, Z);
            set
            {
                Y = value.X;
                Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="Vector2I"/> with the Z and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2I Zx
        {
            readonly get => new(Z, X);
            set
            {
                Z = value.X;
                X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="Vector2I"/> with the Z and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2I Zy
        {
            readonly get => new(Z, Y);
            set
            {
                Z = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="Vector3I"/> with the X, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3I Xzy
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
        /// Gets or sets a <see cref="Vector3I"/> with the Y, X, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3I Yxz
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
        /// Gets or sets a <see cref="Vector3I"/> with the Y, Z, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3I Yzx
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
        /// Gets or sets a <see cref="Vector3I"/> with the Z, X, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3I Zxy
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
        /// Gets or sets a <see cref="Vector3I"/> with the Z, Y, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3I Zyx
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
        /// Gets a <see cref="Vector3"/> object with the same component values as the <see cref="Vector3I"/> instance.
        /// </summary>
        /// <returns>The resulting <see cref="Vector3"/> instance.</returns>
        public readonly Vector3 ToVector3() => new(X, Y, Z);

        /// <summary>
        /// Gets a <see cref="Vector3"/> object with the same component values as the <see cref="Vector3I"/> instance.
        /// </summary>
        /// <param name="input">The given <see cref="Vector3I"/> to convert.</param>
        /// <param name="result">The resulting <see cref="Vector3"/>.</param>
        public static void ToVector3(in Vector3I input, out Vector3 result)
        {
            result.X = input.X;
            result.Y = input.Y;
            result.Z = input.Z;
        }

        /// <summary>
        /// Adds two instances.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static Vector3I operator +(Vector3I left, Vector3I right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            return left;
        }

        /// <summary>
        /// Subtracts two instances.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static Vector3I operator -(Vector3I left, Vector3I right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            return left;
        }

        /// <summary>
        /// Negates an instance.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static Vector3I operator -(Vector3I vec)
        {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            vec.Z = -vec.Z;
            return vec;
        }

        /// <summary>
        /// Multiplies an instance by an integer scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static Vector3I operator *(Vector3I vec, int scale)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            return vec;
        }

        /// <summary>
        /// Multiplies an instance by an integer scalar.
        /// </summary>
        /// <param name="scale">The scalar.</param>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static Vector3I operator *(int scale, Vector3I vec)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            return vec;
        }

        /// <summary>
        /// Component-wise multiplication between the specified instance by a scale vector.
        /// </summary>
        /// <param name="scale">Left operand.</param>
        /// <param name="vec">Right operand.</param>
        /// <returns>Result of multiplication.</returns>
        [Pure]
        public static Vector3I operator *(Vector3I vec, Vector3I scale)
        {
            vec.X *= scale.X;
            vec.Y *= scale.Y;
            vec.Z *= scale.Z;
            return vec;
        }

        /// <summary>
        /// Divides the instance by a scalar using integer division, floor(a/b).
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static Vector3I operator /(Vector3I vec, int scale)
        {
            vec.X /= scale;
            vec.Y /= scale;
            vec.Z /= scale;
            return vec;
        }

        /// <summary>
        /// Component-wise division between the specified instance by a scale vector.
        /// </summary>
        /// <param name="vec">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the division.</returns>
        [Pure]
        public static Vector3I operator /(Vector3I vec, Vector3I scale)
        {
            vec.X /= scale.X;
            vec.Y /= scale.Y;
            vec.Z /= scale.Z;
            return vec;
        }

        /// <summary>
        /// Compares two instances for equality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        public static bool operator ==(Vector3I left, Vector3I right) => left.Equals(right);

        /// <summary>
        /// Compares two instances for inequality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equal right; false otherwise.</returns>
        public static bool operator !=(Vector3I left, Vector3I right) => !(left == right);

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector3i to SatisfactorySaveNet.Vector3.
        /// </summary>
        /// <param name="vec">The Vector3i to convert.</param>
        /// <returns>The resulting Vector3.</returns>
        [Pure]
        public static implicit operator Vector3(Vector3I vec) => new(vec.X, vec.Y, vec.Z);

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector3i to SatisfactorySaveNet.Vector3d.
        /// </summary>
        /// <param name="vec">The Vector3i to convert.</param>
        /// <returns>The resulting Vector3d.</returns>
        [Pure]
        public static implicit operator Vector3D(Vector3I vec) => new(vec.X, vec.Y, vec.Z);

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector3i to SatisfactorySaveNet.Vector3h.
        /// </summary>
        /// <param name="vec">The Vector3i to convert.</param>
        /// <returns>The resulting Vector3h.</returns>
        [Pure]
        public static explicit operator Vector3H(Vector3I vec) => new(vec.X, vec.Y, vec.Z);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3I"/> struct using a tuple containing the component
        /// values.
        /// </summary>
        /// <param name="values">A tuple containing the component values.</param>
        /// <returns>A new instance of the <see cref="Vector3I"/> struct with the given component values.</returns>
        [Pure]
        public static implicit operator Vector3I((int X, int Y, int Z) values) => new(values.X, values.Y, values.Z);

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
                "({0}{3} {1}{3} {2})",
                X.ToString(format, formatProvider),
                Y.ToString(format, formatProvider),
                Z.ToString(format, formatProvider),
                MathHelper.GetListSeparator(formatProvider));
        }

        /// <inheritdoc />
        public override readonly bool Equals(object? obj) => obj is Vector3I vector && Equals(vector);

        /// <inheritdoc />
        public readonly bool Equals(Vector3I other) => X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;

        /// <inheritdoc />
#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z);
#pragma warning restore S2328 // "GetHashCode" should not reference mutable fields

        /// <summary>
        /// Deconstructs the vector into it's individual components.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        [Pure]
        public readonly void Deconstruct(out int x, out int y, out int z)
        {
            x = X;
            y = Y;
            z = Z;
        }
    }
}
