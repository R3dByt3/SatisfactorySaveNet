using SatisfactorySaveNet.Abstracts.Maths.Vector;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace SatisfactorySaveNet.Abstracts.Maths.Geometry
{
    /// <summary>
    /// Defines an axis-aligned 3d box (rectangular prism).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct Box3D : IEquatable<Box3D>, IFormattable
    {
        private Vector3D _min;

        /// <summary>
        /// Gets or sets the minimum boundary of the structure.
        /// </summary>
        public Vector3D Min
        {
            readonly get => _min;
            set
            {
                if (value.X > _max.X)
                {
                    _max.X = value.X;
                }
                if (value.Y > _max.Y)
                {
                    _max.Y = value.Y;
                }
                if (value.Z > _max.Z)
                {
                    _max.Z = value.Z;
                }

                _min = value;
            }
        }

        private Vector3D _max;

        /// <summary>
        /// Gets or sets the maximum boundary of the structure.
        /// </summary>
        public Vector3D Max
        {
            readonly get => _max;
            set
            {
                if (value.X < _min.X)
                {
                    _min.X = value.X;
                }
                if (value.Y < _min.Y)
                {
                    _min.Y = value.Y;
                }
                if (value.Z < _min.Z)
                {
                    _min.Z = value.Z;
                }

                _max = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Box3D"/> struct.
        /// </summary>
        /// <param name="min">The minimum point in 3D space this box encloses.</param>
        /// <param name="max">The maximum point in 3D space this box encloses.</param>
        public Box3D(Vector3D min, Vector3D max)
        {
            _min = Vector3D.ComponentMin(min, max);
            _max = Vector3D.ComponentMax(min, max);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Box3D"/> struct.
        /// </summary>
        /// <param name="minX">The minimum X value to be enclosed.</param>
        /// <param name="minY">The minimum Y value to be enclosed.</param>
        /// <param name="minZ">The minimum Z value to be enclosed.</param>
        /// <param name="maxX">The maximum X value to be enclosed.</param>
        /// <param name="maxY">The maximum Y value to be enclosed.</param>
        /// <param name="maxZ">The maximum Z value to be enclosed.</param>
        public Box3D(double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
            : this(new Vector3D(minX, minY, minZ), new Vector3D(maxX, maxY, maxZ))
        {
        }

        /// <summary>
        /// Gets or sets a vector describing the size of the Box3d structure.
        /// </summary>
        [XmlIgnore]
        public Vector3D Size
        {
            readonly get => Max - Min;
            set
            {
                Vector3D center = Center;
                _min = center - (value * 0.5f);
                _max = center + (value * 0.5f);
            }
        }

        /// <summary>
        /// Gets or sets a vector describing half the size of the box.
        /// </summary>
        [XmlIgnore]
        public Vector3D HalfSize
        {
            readonly get => Size / 2;
            set => Size = value * 2;
        }

        /// <summary>
        /// Gets or sets a vector describing the center of the box.
        /// </summary>
        [XmlIgnore]
        public Vector3D Center
        {
            readonly get => HalfSize + _min;
            set => Translate(value - Center);
        }

        /// <summary>
        /// Returns whether the box contains the specified point (borders inclusive).
        /// </summary>
        /// <param name="point">The point to query.</param>
        /// <returns>Whether this box contains the point.</returns>
        [Pure]
        public readonly bool ContainsInclusive(Vector3D point) => _min.X <= point.X && point.X <= _max.X &&
                   _min.Y <= point.Y && point.Y <= _max.Y &&
                   _min.Z <= point.Z && point.Z <= _max.Z;

        /// <summary>
        /// Returns whether the box contains the specified point (borders exclusive).
        /// </summary>
        /// <param name="point">The point to query.</param>
        /// <returns>Whether this box contains the point.</returns>
        [Pure]
        public readonly bool ContainsExclusive(Vector3D point) => _min.X < point.X && point.X < _max.X &&
                   _min.Y < point.Y && point.Y < _max.Y &&
                   _min.Z < point.Z && point.Z < _max.Z;

        /// <summary>
        /// Returns whether the box contains the specified point.
        /// </summary>
        /// <param name="point">The point to query.</param>
        /// <param name="boundaryInclusive">
        /// Whether points on the box boundary should be recognised as contained as well.
        /// </param>
        /// <returns>Whether this box contains the point.</returns>
        [Pure]
        public readonly bool Contains(Vector3D point, bool boundaryInclusive) => boundaryInclusive ? ContainsInclusive(point) : ContainsExclusive(point);

        /// <summary>
        /// Returns whether the box contains the specified box (borders inclusive).
        /// </summary>
        /// <param name="other">The box to query.</param>
        /// <returns>Whether this box contains the other box.</returns>
        [Pure]
        public readonly bool Contains(Box3D other) => _max.X >= other._min.X && _min.X <= other._max.X &&
                   _max.Y >= other._min.Y && _min.Y <= other._max.Y &&
                   _max.Z >= other._min.Z && _min.Z <= other._max.Z;

        /// <summary>
        /// Returns whether the box contains the specified point (borders inclusive).
        /// </summary>
        /// <param name="point">The point to query.</param>
        /// <returns>Whether this box contains the point.</returns>
        [Pure]
        [Obsolete("This function excludes borders even though it's documentation says otherwise. Use ContainsInclusive and ContainsExclusive for the desired behaviour.")]
        public readonly bool Contains(Vector3D point) => _min.X < point.X && point.X < _max.X &&
                   _min.Y < point.Y && point.Y < _max.Y &&
                   _min.Z < point.Z && point.Z < _max.Z;

        /// <summary>
        /// Returns the distance between the nearest edge and the specified point.
        /// </summary>
        /// <param name="point">The point to find distance for.</param>
        /// <returns>The distance between the specified point and the nearest edge.</returns>
        [Pure]
        public readonly double DistanceToNearestEdge(Vector3D point)
        {
            Vector3D distX = new(
                Math.Max(0f, Math.Max(_min.X - point.X, point.X - _max.X)),
                Math.Max(0f, Math.Max(_min.Y - point.Y, point.Y - _max.Y)),
                Math.Max(0f, Math.Max(_min.Z - point.Z, point.Z - _max.Z)));
            return distX.Length;
        }

        /// <summary>
        /// Translates this Box3d by the given amount.
        /// </summary>
        /// <param name="distance">The distance to translate the box.</param>
        public void Translate(Vector3D distance)
        {
            _min += distance;
            _max += distance;
        }

        /// <summary>
        /// Returns a Box3d translated by the given amount.
        /// </summary>
        /// <param name="distance">The distance to translate the box.</param>
        /// <returns>The translated box.</returns>
        [Pure]
        public readonly Box3D Translated(Vector3D distance)
        {
            // create a local copy of this box
            Box3D box = this;
            box.Translate(distance);
            return box;
        }

        /// <summary>
        /// Scales this Box3d by the given amount.
        /// </summary>
        /// <param name="scale">The scale to scale the box.</param>
        /// <param name="anchor">The anchor to scale the box from.</param>
        public void Scale(Vector3D scale, Vector3D anchor)
        {
            _min = anchor + ((_min - anchor) * scale);
            _max = anchor + ((_max - anchor) * scale);
        }

        /// <summary>
        /// Returns a Box3d scaled by a given amount from an anchor point.
        /// </summary>
        /// <param name="scale">The scale to scale the box.</param>
        /// <param name="anchor">The anchor to scale the box from.</param>
        /// <returns>The scaled box.</returns>
        [Pure]
        public readonly Box3D Scaled(Vector3D scale, Vector3D anchor)
        {
            // create a local copy of this box
            Box3D box = this;
            box.Scale(scale, anchor);
            return box;
        }

        /// <summary>
        /// Inflates this Box3d by the given size in all directions. A negative size will shrink the box to a maximum of -HalfSize.
        /// Use the <see cref="Extend"/> method for the point-encapsulation functionality in SatisfactorySaveNet version 4.8.1 and earlier.
        /// </summary>
        /// <param name="size">The size to inflate by.</param>
        public void Inflate(Vector3D size)
        {
            size = Vector3D.ComponentMax(size, -HalfSize);
            _min -= size;
            _max += size;
        }

        /// <summary>
        /// Inflates this Box3d by the given size in all directions. A negative size will shrink the box to a maximum of -HalfSize.
        /// Use the <see cref="Extended"/> method for the point-encapsulation functionality in SatisfactorySaveNet version 4.8.1 and earlier.
        /// </summary>
        /// <param name="size">The size to inflate by.</param>
        /// <returns>The inflated box.</returns>
        [Pure]
        public readonly Box3D Inflated(Vector3D size)
        {
            // create a local copy of this box
            Box3D box = this;
            box.Inflate(size);
            return box;
        }

        /// <summary>
        /// Extend this Box3d to encapsulate a given point.
        /// </summary>
        /// <param name="point">The point to contain.</param>
        public void Extend(Vector3D point)
        {
            _min = Vector3D.ComponentMin(_min, point);
            _max = Vector3D.ComponentMax(_max, point);
        }

        /// <summary>
        /// Extend this Box3d to encapsulate a given point.
        /// </summary>
        /// <param name="point">The point to contain.</param>
        /// <returns>The inflated box.</returns>
        [Pure]
        public readonly Box3D Extended(Vector3D point)
        {
            // create a local copy of this box
            Box3D box = this;
            box.Extend(point);
            return box;
        }

        /// <summary>
        /// Equality comparator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        public static bool operator ==(Box3D left, Box3D right) => left.Equals(right);

        /// <summary>
        /// Inequality comparator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        public static bool operator !=(Box3D left, Box3D right) => !(left == right);

        /// <inheritdoc/>
        public override readonly bool Equals(object? obj) => obj is Box3D box && Equals(box);

        /// <inheritdoc/>
        public readonly bool Equals(Box3D other) => _min.Equals(other._min) &&
                   _max.Equals(other._max);

        /// <inheritdoc/>
#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
        public override readonly int GetHashCode() => HashCode.Combine(_min, _max);
#pragma warning restore S2328 // "GetHashCode" should not reference mutable fields

        /// <inheritdoc/>
        public override readonly string ToString() => ToString(null, null);

        /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
        public readonly string ToString(string format) => ToString(format, null);

        /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
        public readonly string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);

        /// <inheritdoc/>
        public readonly string ToString(string? format, IFormatProvider? formatProvider) => $"{Min.ToString(format, formatProvider)} - {Max.ToString(format, formatProvider)}";
    }
}
