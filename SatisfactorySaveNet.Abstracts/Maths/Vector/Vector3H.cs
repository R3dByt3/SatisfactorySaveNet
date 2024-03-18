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
    /// 3-component Vector of the Half type. Occupies 6 Byte total.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3H : ISerializable, IEquatable<Vector3H>, IFormattable
    {
        /// <summary>
        /// The X component of the Half3.
        /// </summary>
        public Half X;

        /// <summary>
        /// The Y component of the Half3.
        /// </summary>
        public Half Y;

        /// <summary>
        /// The Z component of the Half3.
        /// </summary>
        public Half Z;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// </summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public Vector3H(Half value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// </summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public Vector3H(float value)
        {
            X = new Half(value);
            Y = new Half(value);
            Z = new Half(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        public Vector3H(Half x, Half y, Half z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// The new Half3 instance will convert the 3 parameters into 16-bit half-precision floating-point.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        public Vector3H(float x, float y, float z)
        {
            X = new Half(x);
            Y = new Half(y);
            Z = new Half(z);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// The new Half3 instance will convert the 3 parameters into 16-bit half-precision floating-point.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector3H(float x, float y, float z, bool throwOnError)
        {
            X = new Half(x, throwOnError);
            Y = new Half(y, throwOnError);
            Z = new Half(z, throwOnError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector3"/> to convert.</param>
        public Vector3H(Vector3 v)
        {
            X = new Half(v.X);
            Y = new Half(v.Y);
            Z = new Half(v.Z);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector3"/> to convert.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector3H(Vector3 v, bool throwOnError)
        {
            X = new Half(v.X, throwOnError);
            Y = new Half(v.Y, throwOnError);
            Z = new Half(v.Z, throwOnError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector3"/> to convert.</param>
        public Vector3H(in Vector3 v)
        {
            X = new Half(v.X);
            Y = new Half(v.Y);
            Z = new Half(v.Z);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector3"/> to convert.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector3H(in Vector3 v, bool throwOnError)
        {
            X = new Half(v.X, throwOnError);
            Y = new Half(v.Y, throwOnError);
            Z = new Half(v.Z, throwOnError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector3D"/> to convert.</param>
        public Vector3H(Vector3D v)
        {
            X = new Half(v.X);
            Y = new Half(v.Y);
            Z = new Half(v.Z);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector3D"/> to convert.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector3H(Vector3D v, bool throwOnError)
        {
            X = new Half(v.X, throwOnError);
            Y = new Half(v.Y, throwOnError);
            Z = new Half(v.Z, throwOnError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector3D"/> to convert.</param>
        public Vector3H(in Vector3D v)
        {
            X = new Half(v.X);
            Y = new Half(v.Y);
            Z = new Half(v.Z);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector3D"/> to convert.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector3H(in Vector3D v, bool throwOnError)
        {
            X = new Half(v.X, throwOnError);
            Y = new Half(v.Y, throwOnError);
            Z = new Half(v.Z, throwOnError);
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2h with the X and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2H Xy
        {
            get => Unsafe.As<Vector3H, Vector2H>(ref this);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2h with the X and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2H Xz
        {
            readonly get => new(X, Z);
            set
            {
                X = value.X;
                Z = value.Y;
            }
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
        /// Gets or sets an SatisfactorySaveNet.Vector2h with the Y and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2H Yz
        {
            readonly get => new(Y, Z);
            set
            {
                Y = value.X;
                Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2h with the Z and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2H Zx
        {
            readonly get => new(Z, X);
            set
            {
                Z = value.X;
                X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2h with the Z and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2H Zy
        {
            readonly get => new(Z, Y);
            set
            {
                Z = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the X, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Xzy
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the Y, X, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Yxz
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the Y, Z, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Yzx
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the Z, X, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Zxy
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the Z, Y, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Zyx
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
        /// Returns this Half3 instance's contents as Vector3.
        /// </summary>
        /// <returns>The vector.</returns>
        public readonly Vector3 ToVector3()
        {
            return new(X, Y, Z);
        }

        /// <summary>
        /// Returns this Half3 instance's contents as Vector3d.
        /// </summary>
        /// <returns>The vector.</returns>
        public readonly Vector3D ToVector3d()
        {
            return new(X, Y, Z);
        }

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector3h to SatisfactorySaveNet.Vector3.
        /// </summary>
        /// <param name="vec">The Vector3h to convert.</param>
        /// <returns>The resulting Vector3.</returns>
        [Pure]
        public static implicit operator Vector3(Vector3H vec)
        {
            return new(vec.X, vec.Y, vec.Z);
        }

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector3h to SatisfactorySaveNet.Vector3d.
        /// </summary>
        /// <param name="vec">The Vector3h to convert.</param>
        /// <returns>The resulting Vector3d.</returns>
        [Pure]
        public static implicit operator Vector3D(Vector3H vec)
        {
            return new(vec.X, vec.Y, vec.Z);
        }

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector3h to SatisfactorySaveNet.Vector3i.
        /// </summary>
        /// <param name="vec">The Vector3h to convert.</param>
        /// <returns>The resulting Vector3i.</returns>
        [Pure]
        public static explicit operator Vector3I(Vector3H vec)
        {
            return new((int) vec.X, (int) vec.Y, (int) vec.Z);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct using a tuple containing the component
        /// values.
        /// </summary>
        /// <param name="values">A tuple containing the component values.</param>
        /// <returns>A new instance of the <see cref="Vector3H"/> struct with the given component values.</returns>
        [Pure]
        public static implicit operator Vector3H((Half X, Half Y, Half Z) values)
        {
            return new(values.X, values.Y, values.Z);
        }

        /// <summary>
        /// Compares two instances for equality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        public static bool operator ==(Vector3H left, Vector3H right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two instances for inequality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equal right; false otherwise.</returns>
        public static bool operator !=(Vector3H left, Vector3H right)
        {
            return !(left == right);
        }

        /// <summary>
        /// The size in bytes for an instance of the Half3 struct is 6.
        /// </summary>
        public static readonly int SizeInBytes = Unsafe.SizeOf<Vector3H>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3H"/> struct.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Vector3H(SerializationInfo info, StreamingContext context)
        {
            var xValue = info.GetValue("X", typeof(Half)) ?? throw new ArgumentOutOfRangeException(nameof(info), null, "X must be not null");
            var yValue = info.GetValue("Y", typeof(Half)) ?? throw new ArgumentOutOfRangeException(nameof(info), null, "Y must be not null");
            var zValue = info.GetValue("Z", typeof(Half)) ?? throw new ArgumentOutOfRangeException(nameof(info), null, "Z must be not null");

            X = (Half) xValue;
            Y = (Half) yValue;
            Z = (Half) zValue;
        }

        /// <inheritdoc/>
        public readonly void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X", X);
            info.AddValue("Y", Y);
            info.AddValue("Z", Z);
        }

        /// <summary>
        /// Updates the X,Y and Z components of this instance by reading from a Stream.
        /// </summary>
        /// <param name="bin">A BinaryReader instance associated with an open Stream.</param>
        public void FromBinaryStream(BinaryReader bin)
        {
            X = Half.FromBinaryStream(bin);
            Y = Half.FromBinaryStream(bin);
            Z = Half.FromBinaryStream(bin);
        }

        /// <summary>
        /// Writes the X,Y and Z components of this instance into a Stream.
        /// </summary>
        /// <param name="bin">A BinaryWriter instance associated with an open Stream.</param>
        public readonly void ToBinaryStream(BinaryWriter bin)
        {
            X.ToBinaryStream(bin);
            Y.ToBinaryStream(bin);
            Z.ToBinaryStream(bin);
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

        /// <inheritdoc/>
        public readonly override bool Equals(object? obj)
        {
            return obj is Vector3H vector && Equals(vector);
        }

        /// <inheritdoc/>
        public readonly bool Equals(Vector3H other)
        {
            return X.Equals(other.X) &&
                   Y.Equals(other.Y) &&
                   Z.Equals(other.Z);
        }

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        /// <summary>
        /// Returns the Half3 as an array of bytes.
        /// </summary>
        /// <param name="h">The Half3 to convert.</param>
        /// <returns>The input as byte array.</returns>
        [Pure]
        public static byte[] GetBytes(Vector3H h)
        {
            var result = new byte[SizeInBytes];

            var temp = Half.GetBytes(h.X);
            result[0] = temp[0];
            result[1] = temp[1];
            temp = Half.GetBytes(h.Y);
            result[2] = temp[0];
            result[3] = temp[1];
            temp = Half.GetBytes(h.Z);
            result[4] = temp[0];
            result[5] = temp[1];

            return result;
        }

        /// <summary>
        /// Converts an array of bytes into Half3.
        /// </summary>
        /// <param name="value">A Half3 in it's byte[] representation.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A new Half3 instance.</returns>
        [Pure]
        public static Vector3H FromBytes(byte[] value, int startIndex)
        {
            return new Vector3H(
                Half.FromBytes(value, startIndex),
                Half.FromBytes(value, startIndex + 2),
                Half.FromBytes(value, startIndex + 4));
        }

        /// <summary>
        /// Deconstructs the vector into it's individual components.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        [Pure]
        public readonly void Deconstruct(out Half x, out Half y, out Half z)
        {
            x = X;
            y = Y;
            z = Z;
        }
    }
}
