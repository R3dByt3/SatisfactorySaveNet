using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Half = SatisfactorySaveNet.Abstracts.Maths.Data.Half;

namespace SatisfactorySaveNet.Abstracts.Maths.Vector
{
    /// <summary>
    /// 2-component Vector of the Half type. Occupies 4 Byte total.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2H : ISerializable, IEquatable<Vector2H>, IFormattable
    {
        /// <summary>
        /// The X component of the Half2.
        /// </summary>
        public Half X;

        /// <summary>
        /// The Y component of the Half2.
        /// </summary>
        public Half Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public Vector2H(Half value)
        {
            X = value;
            Y = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public Vector2H(float value)
        {
            X = new Half(value);
            Y = new Half(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        public Vector2H(Half x, Half y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        public Vector2H(float x, float y)
        {
            X = new Half(x);
            Y = new Half(y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector2H(float x, float y, bool throwOnError)
        {
            X = new Half(x, throwOnError);
            Y = new Half(y, throwOnError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector2"/> to convert.</param>
        public Vector2H(Vector2 v)
        {
            X = new Half(v.X);
            Y = new Half(v.Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector2"/> to convert.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector2H(Vector2 v, bool throwOnError)
        {
            X = new Half(v.X, throwOnError);
            Y = new Half(v.Y, throwOnError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector2"/> to convert.</param>
        public Vector2H(in Vector2 v)
        {
            X = new Half(v.X);
            Y = new Half(v.Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector2"/> to convert.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector2H(in Vector2 v, bool throwOnError)
        {
            X = new Half(v.X, throwOnError);
            Y = new Half(v.Y, throwOnError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector2D"/> to convert.</param>
        public Vector2H(Vector2D v)
        {
            X = new Half(v.X);
            Y = new Half(v.Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector2D"/> to convert.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector2H(Vector2D v, bool throwOnError)
        {
            X = new Half(v.X, throwOnError);
            Y = new Half(v.Y, throwOnError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector2D"/> to convert.</param>
        public Vector2H(in Vector2D v)
        {
            X = new Half(v.X);
            Y = new Half(v.Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector2D"/> to convert.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector2H(in Vector2D v, bool throwOnError)
        {
            X = new Half(v.X, throwOnError);
            Y = new Half(v.Y, throwOnError);
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2h with the Y and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2H Yx
        {
            readonly get => new(Y, X);
            set
            {
                Y = value.X;
                X = value.Y;
            }
        }

        /// <summary>
        /// Returns this Half2 instance's contents as Vector2.
        /// </summary>
        /// <returns>The vector.</returns>
        public readonly Vector2 ToVector2()
        {
            return new(X, Y);
        }

        /// <summary>
        /// Returns this Half2 instance's contents as Vector2d.
        /// </summary>
        /// <returns>The vector.</returns>
        public readonly Vector2D ToVector2d()
        {
            return new(X, Y);
        }

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector2h to SatisfactorySaveNet.Vector2.
        /// </summary>
        /// <param name="vec">The Vector2h to convert.</param>
        /// <returns>The resulting Vector2.</returns>
        [Pure]
        public static implicit operator Vector2(Vector2H vec)
        {
            return new(vec.X, vec.Y);
        }

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector2h to SatisfactorySaveNet.Vector2d.
        /// </summary>
        /// <param name="vec">The Vector2h to convert.</param>
        /// <returns>The resulting Vector2d.</returns>
        [Pure]
        public static implicit operator Vector2D(Vector2H vec)
        {
            return new(vec.X, vec.Y);
        }

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector2h to SatisfactorySaveNet.Vector2i.
        /// </summary>
        /// <param name="vec">The Vector2h to convert.</param>
        /// <returns>The resulting Vector2i.</returns>
        [Pure]
        public static explicit operator Vector2I(Vector2H vec)
        {
            return new((int) vec.X, (int) vec.Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct using a tuple containing the component
        /// values.
        /// </summary>
        /// <param name="values">A tuple containing the component values.</param>
        /// <returns>A new instance of the <see cref="Vector2H"/> struct with the given component values.</returns>
        [Pure]
        public static implicit operator Vector2H((Half X, Half Y) values)
        {
            return new(values.X, values.Y);
        }

        /// <summary>
        /// Compares the specified instances for equality.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>True if both instances are equal; false otherwise.</returns>
        public static bool operator ==(Vector2H left, Vector2H right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares the specified instances for inequality.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>True if both instances are not equal; false otherwise.</returns>
        public static bool operator !=(Vector2H left, Vector2H right)
        {
            return !(left == right);
        }

        /// <summary>
        /// The size in bytes for an instance of the Half2 struct is 4.
        /// </summary>
        public static readonly int SizeInBytes = Unsafe.SizeOf<Vector2H>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2H"/> struct.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Vector2H(SerializationInfo info, StreamingContext context)
        {
            var xValue = info.GetValue("X", typeof(Half)) ?? throw new ArgumentOutOfRangeException(nameof(info), null, "X must be not null");
            var yValue = info.GetValue("Y", typeof(Half)) ?? throw new ArgumentOutOfRangeException(nameof(info), null, "Y must be not null");
            X = (Half) xValue;
            Y = (Half) yValue;
        }

        /// <inheritdoc/>
        public readonly void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X", X);
            info.AddValue("Y", Y);
        }

        /// <summary>
        /// Updates the X and Y components of this instance by reading from a Stream.
        /// </summary>
        /// <param name="bin">A BinaryReader instance associated with an open Stream.</param>
        public void FromBinaryStream(BinaryReader bin)
        {
            X = Half.FromBinaryStream(bin);
            Y = Half.FromBinaryStream(bin);
        }

        /// <summary>
        /// Writes the X and Y components of this instance into a Stream.
        /// </summary>
        /// <param name="bin">A BinaryWriter instance associated with an open Stream.</param>
        public readonly void ToBinaryStream(BinaryWriter bin)
        {
            X.ToBinaryStream(bin);
            Y.ToBinaryStream(bin);
        }

        /// <inheritdoc/>
        public readonly override string ToString()
        {
            return ToString(null, null);
        }

        /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
        public readonly string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
        public readonly string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }

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
        public readonly override bool Equals(object? obj)
        {
            return obj is Vector2H vector && Equals(vector);
        }

        /// <inheritdoc/>
        public readonly bool Equals(Vector2H other)
        {
            return X.Equals(other.X) &&
                   Y.Equals(other.Y);
        }

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        /// <summary>
        /// Returns the Half2 as an array of bytes.
        /// </summary>
        /// <param name="h">The Half2 to convert.</param>
        /// <returns>The input as byte array.</returns>
        [Pure]
        public static byte[] GetBytes(Vector2H h)
        {
            var result = new byte[SizeInBytes];

            var temp = Half.GetBytes(h.X);
            result[0] = temp[0];
            result[1] = temp[1];
            temp = Half.GetBytes(h.Y);
            result[2] = temp[0];
            result[3] = temp[1];

            return result;
        }

        /// <summary>
        /// Converts an array of bytes into Half2.
        /// </summary>
        /// <param name="value">A Half2 in it's byte[] representation.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A new Half2 instance.</returns>
        [Pure]
        public static Vector2H FromBytes(byte[] value, int startIndex)
        {
            return new(
                Half.FromBytes(value, startIndex),
                Half.FromBytes(value, startIndex + 2));
        }

        /// <summary>
        /// Deconstructs the vector into it's individual components.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        [Pure]
        public readonly void Deconstruct(out Half x, out Half y)
        {
            x = X;
            y = Y;
        }
    }
}
