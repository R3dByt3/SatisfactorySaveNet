using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Half = SatisfactorySaveNet.Abstracts.Maths.Data.Half;

namespace SatisfactorySaveNet.Abstracts.Maths.Vector
{
    /// <summary>
    /// 4-component Vector of the Half type. Occupies 8 Byte total.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4H : ISerializable, IEquatable<Vector4H>, IFormattable
    {
        /// <summary>
        /// The X component of the Half4.
        /// </summary>
        public Half X;

        /// <summary>
        /// The Y component of the Half4.
        /// </summary>
        public Half Y;

        /// <summary>
        /// The Z component of the Half4.
        /// </summary>
        public Half Z;

        /// <summary>
        /// The W component of the Half4.
        /// </summary>
        public Half W;

        /// <summary>
        /// Defines the size of the Vector4d struct in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Unsafe.SizeOf<Vector4H>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public Vector4H(Half value)
        {
            X = value;
            Y = value;
            Z = value;
            W = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public Vector4H(float value)
        {
            X = new Half(value);
            Y = new Half(value);
            Z = new Half(value);
            W = new Half(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        /// <param name="w">The W component of the vector.</param>
        public Vector4H(Half x, Half y, Half z, Half w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        /// <param name="w">The W component of the vector.</param>
        public Vector4H(float x, float y, float z, float w)
        {
            X = new Half(x);
            Y = new Half(y);
            Z = new Half(z);
            W = new Half(w);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        /// <param name="w">The W component of the vector.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector4H(float x, float y, float z, float w, bool throwOnError)
        {
            X = new Half(x, throwOnError);
            Y = new Half(y, throwOnError);
            Z = new Half(z, throwOnError);
            W = new Half(w, throwOnError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector4"/> to convert.</param>
        public Vector4H(Vector4 v)
        {
            X = new Half(v.X);
            Y = new Half(v.Y);
            Z = new Half(v.Z);
            W = new Half(v.W);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector4"/> to convert.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector4H(Vector4 v, bool throwOnError)
        {
            X = new Half(v.X, throwOnError);
            Y = new Half(v.Y, throwOnError);
            Z = new Half(v.Z, throwOnError);
            W = new Half(v.W, throwOnError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector4"/> to convert.</param>
        public Vector4H(in Vector4 v)
        {
            X = new Half(v.X);
            Y = new Half(v.Y);
            Z = new Half(v.Z);
            W = new Half(v.W);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector4"/> to convert.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector4H(in Vector4 v, bool throwOnError)
        {
            X = new Half(v.X, throwOnError);
            Y = new Half(v.Y, throwOnError);
            Z = new Half(v.Z, throwOnError);
            W = new Half(v.W, throwOnError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector4D"/> to convert.</param>
        public Vector4H(Vector4D v)
        {
            X = new Half(v.X);
            Y = new Half(v.Y);
            Z = new Half(v.Z);
            W = new Half(v.W);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector4D"/> to convert.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector4H(Vector4D v, bool throwOnError)
        {
            X = new Half(v.X, throwOnError);
            Y = new Half(v.Y, throwOnError);
            Z = new Half(v.Z, throwOnError);
            W = new Half(v.W, throwOnError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector4D"/> to convert.</param>
        public Vector4H(in Vector4D v)
        {
            X = new Half(v.X);
            Y = new Half(v.Y);
            Z = new Half(v.Z);
            W = new Half(v.W);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="v">The <see cref="Vector4D"/> to convert.</param>
        /// <param name="throwOnError">Enable checks that will throw if the conversion result is not meaningful.</param>
        public Vector4H(in Vector4D v, bool throwOnError)
        {
            X = new Half(v.X, throwOnError);
            Y = new Half(v.Y, throwOnError);
            Z = new Half(v.Z, throwOnError);
            W = new Half(v.W, throwOnError);
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2h with the X and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2H Xy
        {
            get => Unsafe.As<Vector4H, Vector2H>(ref this);
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
        /// Gets or sets an SatisfactorySaveNet.Vector2h with the X and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2H Xw
        {
            readonly get => new(X, W);
            set
            {
                X = value.X;
                W = value.Y;
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
        /// Gets or sets an SatisfactorySaveNet.Vector2h with the Y and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2H Yw
        {
            readonly get => new(Y, W);
            set
            {
                Y = value.X;
                W = value.Y;
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
        /// Gets or sets an SatisfactorySaveNet.Vector2h with the Z and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2H Zw
        {
            readonly get => new(Z, W);
            set
            {
                Z = value.X;
                W = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2h with the W and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2H Wx
        {
            readonly get => new(W, X);
            set
            {
                W = value.X;
                X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2h with the W and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2H Wy
        {
            readonly get => new(W, Y);
            set
            {
                W = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector2h with the W and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector2H Wz
        {
            readonly get => new(W, Z);
            set
            {
                W = value.X;
                Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the X, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Xyz
        {
            get => Unsafe.As<Vector4H, Vector3H>(ref this);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the X, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Xyw
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the X, Z, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Xzw
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the X, W, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Xwy
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the X, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Xwz
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the Y, X, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Yxw
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the Y, Z, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Yzw
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the Y, W, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Ywx
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the Y, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Ywz
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the Z, X, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Zxw
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the Z, Y, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Zyw
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the Z, W, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Zwx
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the Z, W, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Zwy
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the W, X, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Wxy
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the W, X, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Wxz
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the W, Y, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Wyx
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the W, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Wyz
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the W, Z, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Wzx
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
        /// Gets or sets an SatisfactorySaveNet.Vector3h with the W, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector3H Wzy
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the X, Y, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Xywz
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the X, Z, Y, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Xzyw
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the X, Z, W, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Xzwy
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the X, W, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Xwyz
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the X, W, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Xwzy
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Y, X, Z, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Yxzw
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Y, X, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Yxwz
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Y, Y, Z, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Yyzw
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Y, Y, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Yywz
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Y, Z, X, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Yzxw
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Y, Z, W, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Yzwx
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Y, W, X, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Ywxz
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Y, W, Z, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Ywzx
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Z, X, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Zxyw
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Z, X, W, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Zxwy
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Z, Y, X, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Zyxw
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Z, Y, W, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Zywx
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Z, W, X, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Zwxy
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Z, W, Y, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Zwyx
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the Z, W, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Zwzy
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the W, X, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Wxyz
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the W, X, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Wxzy
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the W, Y, X, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Wyxz
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the W, Y, Z, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Wyzx
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the W, Z, X, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Wzxy
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the W, Z, Y, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Wzyx
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
        /// Gets or sets an SatisfactorySaveNet.Vector4h with the W, Z, Y, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public Vector4H Wzyw
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
        /// Returns this Half4 instance's contents as Vector4.
        /// </summary>
        /// <returns>The vector.</returns>
        public readonly Vector4 ToVector4() => new(X, Y, Z, W);

        /// <summary>
        /// Returns this Half4 instance's contents as Vector4d.
        /// </summary>
        /// <returns>The vector.</returns>
        public readonly Vector4D ToVector4d() => new(X, Y, Z, W);

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector4 to SatisfactorySaveNet.Half4.
        /// </summary>
        /// <param name="v4f">The Vector4 to convert.</param>
        /// <returns>The resulting Half vector.</returns>
        [Pure]
        public static explicit operator Vector4H(Vector4 v4f) => new(v4f);

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector4d to SatisfactorySaveNet.Half4.
        /// </summary>
        /// <param name="v4d">The Vector4d to convert.</param>
        /// <returns>The resulting Half vector.</returns>
        [Pure]
        public static explicit operator Vector4H(Vector4D v4d) => new(v4d);

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector4h to SatisfactorySaveNet.Vector4.
        /// </summary>
        /// <param name="vec">The Vector4h to convert.</param>
        /// <returns>The resulting Vector4.</returns>
        [Pure]
        public static implicit operator Vector4(Vector4H vec) => new(vec.X, vec.Y, vec.Z, vec.W);

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector4h to SatisfactorySaveNet.Vector4d.
        /// </summary>
        /// <param name="vec">The Vector4h to convert.</param>
        /// <returns>The resulting Vector4d.</returns>
        [Pure]
        public static implicit operator Vector4D(Vector4H vec) => new(vec.X, vec.Y, vec.Z, vec.W);

        /// <summary>
        /// Converts SatisfactorySaveNet.Vector4h to SatisfactorySaveNet.Vector4i.
        /// </summary>
        /// <param name="vec">The Vector4h to convert.</param>
        /// <returns>The resulting Vector4i.</returns>
        [Pure]
        public static explicit operator Vector4I(Vector4H vec) => new((int)vec.X, (int)vec.Y, (int)vec.Z, (int)vec.W);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct using a tuple containing the component
        /// values.
        /// </summary>
        /// <param name="values">A tuple containing the component values.</param>
        /// <returns>A new instance of the <see cref="Vector4H"/> struct with the given component values.</returns>
        [Pure]
        public static implicit operator Vector4H((Half X, Half Y, Half Z, Half W) values) => new(values.X, values.Y, values.Z, values.W);

        /// <summary>
        /// Compares two instances for equality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        public static bool operator ==(Vector4H left, Vector4H right) => left.Equals(right);

        /// <summary>
        /// Compares two instances for inequality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equa lright; false otherwise.</returns>
        public static bool operator !=(Vector4H left, Vector4H right) => !(left == right);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4H"/> struct.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Vector4H(SerializationInfo info, StreamingContext context)
        {
            var xValue = info.GetValue("X", typeof(Half)) ?? throw new ArgumentOutOfRangeException(nameof(info), null, "X must be not null");
            var yValue = info.GetValue("Y", typeof(Half)) ?? throw new ArgumentOutOfRangeException(nameof(info), null, "Y must be not null");
            var zValue = info.GetValue("Z", typeof(Half)) ?? throw new ArgumentOutOfRangeException(nameof(info), null, "Z must be not null");
            var WValue = info.GetValue("W", typeof(Half)) ?? throw new ArgumentOutOfRangeException(nameof(info), null, "W must be not null");
            X = (Half)xValue;
            Y = (Half)yValue;
            Z = (Half)zValue;
            W = (Half)WValue;
        }

        /// <inheritdoc />
        public readonly void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X", X);
            info.AddValue("Y", Y);
            info.AddValue("Z", Z);
            info.AddValue("W", W);
        }

        /// <summary>
        /// Updates the X,Y,Z and W components of this instance by reading from a Stream.
        /// </summary>
        /// <param name="bin">A BinaryReader instance associated with an open Stream.</param>
        public void FromBinaryStream(BinaryReader bin)
        {
            X = Half.FromBinaryStream(bin);
            Y = Half.FromBinaryStream(bin);
            Z = Half.FromBinaryStream(bin);
            W = Half.FromBinaryStream(bin);
        }

        /// <summary>
        /// Writes the X,Y,Z and W components of this instance into a Stream.
        /// </summary>
        /// <param name="bin">A BinaryWriter instance associated with an open Stream.</param>
        public readonly void ToBinaryStream(BinaryWriter bin)
        {
            X.ToBinaryStream(bin);
            Y.ToBinaryStream(bin);
            Z.ToBinaryStream(bin);
            W.ToBinaryStream(bin);
        }

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

        /// <inheritdoc/>
        public override readonly bool Equals(object? obj) => obj is Vector4H vector && Equals(vector);

        /// <inheritdoc/>
        public readonly bool Equals(Vector4H other) => X.Equals(other.X) &&
                   Y.Equals(other.Y) &&
                   Z.Equals(other.Z) &&
                   W.Equals(other.W);

        /// <inheritdoc/>
#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z, W);
#pragma warning restore S2328 // "GetHashCode" should not reference mutable fields

        /// <summary>
        /// Returns the Half4 as an array of bytes.
        /// </summary>
        /// <param name="h">The Half4 to convert.</param>
        /// <returns>The input as byte array.</returns>
        [Pure]
        public static byte[] GetBytes(Vector4H h)
        {
            byte[] result = new byte[SizeInBytes];

            byte[] temp = Half.GetBytes(h.X);
            result[0] = temp[0];
            result[1] = temp[1];
            temp = Half.GetBytes(h.Y);
            result[2] = temp[0];
            result[3] = temp[1];
            temp = Half.GetBytes(h.Z);
            result[4] = temp[0];
            result[5] = temp[1];
            temp = Half.GetBytes(h.W);
            result[6] = temp[0];
            result[7] = temp[1];

            return result;
        }

        /// <summary>
        /// Converts an array of bytes into Half4.
        /// </summary>
        /// <param name="value">A Half4 in it's byte[] representation.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A new Half4 instance.</returns>
        [Pure]
        public static Vector4H FromBytes(byte[] value, int startIndex)
        {
            return new Vector4H(
                Half.FromBytes(value, startIndex),
                Half.FromBytes(value, startIndex + 2),
                Half.FromBytes(value, startIndex + 4),
                Half.FromBytes(value, startIndex + 6));
        }

        /// <summary>
        /// Deconstructs the vector into it's individual components.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        /// <param name="w">The W component of the vector.</param>
        [Pure]
        public readonly void Deconstruct(out Half x, out Half y, out Half z, out Half w)
        {
            x = X;
            y = Y;
            z = Z;
            w = W;
        }
    }
}
