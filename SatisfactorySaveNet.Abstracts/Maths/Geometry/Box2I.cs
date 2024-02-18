using SatisfactorySaveNet.Abstracts.Maths.Vector;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace SatisfactorySaveNet.Abstracts.Maths.Geometry
{
    /// <summary>
    /// Defines an axis-aligned 2d box (rectangle).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct Box2I : IEquatable<Box2I>, IFormattable
    {
        /// <summary>
        /// An empty box with Min (0, 0) and Max (0, 0).
        /// </summary>
        public static readonly Box2I Empty = new(0, 0, 0, 0);

        private Vector2I _min;

        /// <summary>
        /// Gets or sets the minimum boundary of the structure.
        /// </summary>
        public Vector2I Min
        {
            readonly get => _min;
            set
            {
                _max = Vector2I.ComponentMax(_max, value);
                _min = value;
            }
        }

        private Vector2I _max;

        /// <summary>
        /// Gets or sets the maximum boundary of the structure.
        /// </summary>
        public Vector2I Max
        {
            readonly get => _max;
            set
            {
                _min = Vector2I.ComponentMin(_min, value);
                _max = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Box2I"/> struct.
        /// </summary>
        /// <param name="min">The minimum point on the XY plane this box encloses.</param>
        /// <param name="max">The maximum point on the XY plane this box encloses.</param>
        public Box2I(Vector2I min, Vector2I max)
        {
            _min = Vector2I.ComponentMin(min, max);
            _max = Vector2I.ComponentMax(min, max);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Box2I"/> struct.
        /// </summary>
        /// <param name="minX">The minimum X value to be enclosed.</param>
        /// <param name="minY">The minimum Y value to be enclosed.</param>
        /// <param name="maxX">The maximum X value to be enclosed.</param>
        /// <param name="maxY">The maximum Y value to be enclosed.</param>
        public Box2I(int minX, int minY, int maxX, int maxY)
            : this(new Vector2I(minX, minY), new Vector2I(maxX, maxY))
        {
        }

        /// <summary>
        /// Gets a vector describing the size of the Box2i structure.
        /// </summary>
        [XmlIgnore]
        public readonly Vector2I Size => Max - Min;

        /// <summary>
        /// Gets or sets a vector describing half the size of the box.
        /// </summary>
        [XmlIgnore]
        public Vector2I HalfSize
        {
            readonly get => Size / 2;
            set
            {
                Vector2I center = new((int)Center.X, (int)Center.Y);
                _min = center - value;
                _max = center + value;
            }
        }

        /// <summary>
        /// Gets a vector describing the center of the box.
        /// </summary>
        /// to avoid annoying off-by-one errors in box placement, no setter is provided for this property
        [XmlIgnore]
        public readonly Vector2 Center => _min + ((_max - _min).ToVector2() * 0.5f);

        /// <summary>
        /// Returns whether the box contains the specified point (borders inclusive).
        /// </summary>
        /// <param name="point">The point to query.</param>
        /// <returns>Whether this box contains the point.</returns>
        [Pure]
        public readonly bool ContainsInclusive(Vector2I point) => _min.X <= point.X && point.X <= _max.X &&
                   _min.Y <= point.Y && point.Y <= _max.Y;

        /// <summary>
        /// Returns whether the box contains the specified point (borders exclusive).
        /// </summary>
        /// <param name="point">The point to query.</param>
        /// <returns>Whether this box contains the point.</returns>
        [Pure]
        public readonly bool ContainsExclusive(Vector2I point) => _min.X < point.X && point.X < _max.X &&
                   _min.Y < point.Y && point.Y < _max.Y;

        /// <summary>
        /// Returns whether the box contains the specified point.
        /// </summary>
        /// <param name="point">The point to query.</param>
        /// <param name="boundaryInclusive">
        /// Whether points on the box boundary should be recognised as contained as well.
        /// </param>
        /// <returns>Whether this box contains the point.</returns>
        [Pure]
        public readonly bool Contains(Vector2I point, bool boundaryInclusive) => boundaryInclusive ? ContainsInclusive(point) : ContainsExclusive(point);

        /// <summary>
        /// Returns whether the box contains the specified box (borders inclusive).
        /// </summary>
        /// <param name="other">The box to query.</param>
        /// <returns>Whether this box contains the other box.</returns>
        [Pure]
        public readonly bool Contains(Box2I other) => _max.X >= other._min.X && _min.X <= other._max.X &&
                   _max.Y >= other._min.Y && _min.Y <= other._max.Y;

        /// <summary>
        /// Returns whether the box contains the specified point (borders inclusive).
        /// </summary>
        /// <param name="point">The point to query.</param>
        /// <returns>Whether this box contains the point.</returns>
        [Pure]
        [Obsolete("This function excludes borders even though it's documentation says otherwise. Use ContainsInclusive and ContainsExclusive for the desired behaviour.")]
        public readonly bool Contains(Vector2I point) => _min.X < point.X && point.X < _max.X &&
                   _min.Y < point.Y && point.Y < _max.Y;

        /// <summary>
        /// Creates a rectangle that represents the intersection between a and
        /// b. If there is no intersection, a empty <see cref="Box2I"/> is returned.
        /// </summary>
        /// <param name="a">First rectangle to intersect.</param>
        /// <param name="b">Second rectangle to intersect.</param>
        /// <returns>The <see cref="Box2I"/> that represents the intersection of both Box2i.</returns>
        public static Box2I Intersect(Box2I a, Box2I b)
        {
            Vector2I min = Vector2I.ComponentMax(a.Min, b.Min);
            Vector2I max = Vector2I.ComponentMin(a.Max, b.Max);

            return max.X >= min.X && max.Y >= min.Y ? new Box2I(min, max) : Empty;
        }

        /// <summary>
        /// Returns the distance between the nearest edge and the specified point.
        /// </summary>
        /// <param name="point">The point to find distance for.</param>
        /// <returns>The distance between the specified point and the nearest edge.</returns>
        [Pure]
        public readonly float DistanceToNearestEdge(Vector2I point)
        {
            Vector2 dist = new(
                Math.Max(0f, Math.Max(_min.X - point.X, point.X - _max.X)),
                Math.Max(0f, Math.Max(_min.Y - point.Y, point.Y - _max.Y)));
            return dist.Length;
        }

        /// <summary>
        /// Translates this Box2i by the given amount.
        /// </summary>
        /// <param name="distance">The distance to translate the box.</param>
        public void Translate(Vector2I distance)
        {
            _min += distance;
            _max += distance;
        }

        /// <summary>
        /// Returns a Box2i translated by the given amount.
        /// </summary>
        /// <param name="distance">The distance to translate the box.</param>
        /// <returns>The translated box.</returns>
        [Pure]
        public readonly Box2I Translated(Vector2I distance)
        {
            // create a local copy of this box
            Box2I box = this;
            box.Translate(distance);
            return box;
        }

        /// <summary>
        /// Scales this Box2i by the given amount.
        /// </summary>
        /// <param name="scale">The scale to scale the box.</param>
        /// <param name="anchor">The anchor to scale the box from.</param>
        public void Scale(Vector2I scale, Vector2I anchor)
        {
            _min = anchor + ((_min - anchor) * scale);
            _max = anchor + ((_max - anchor) * scale);
        }

        /// <summary>
        /// Returns a Box2i scaled by a given amount from an anchor point.
        /// </summary>
        /// <param name="scale">The scale to scale the box.</param>
        /// <param name="anchor">The anchor to scale the box from.</param>
        /// <returns>The scaled box.</returns>
        [Pure]
        public readonly Box2I Scaled(Vector2I scale, Vector2I anchor)
        {
            // create a local copy of this box
            Box2I box = this;
            box.Scale(scale, anchor);
            return box;
        }

        /// <summary>
        /// Inflates this Box2i by the given size in all directions. A negative size will shrink the box to a maximum of -HalfSize.
        /// Use the <see cref="Extend"/> method for the point-encapsulation functionality in SatisfactorySaveNet version 4.8.1 and earlier.
        /// </summary>
        /// <param name="size">The size to inflate by.</param>
        public void Inflate(Vector2I size)
        {
            size = Vector2I.ComponentMax(size, -HalfSize);
            _min -= size;
            _max += size;
        }

        /// <summary>
        /// Inflates this Box2i by the given size in all directions. A negative size will shrink the box to a maximum of -HalfSize.
        /// Use the <see cref="Extended"/> method for the point-encapsulation functionality in SatisfactorySaveNet version 4.8.1 and earlier.
        /// </summary>
        /// <param name="size">The size to inflate by.</param>
        /// <returns>The inflated box.</returns>
        [Pure]
        public readonly Box2I Inflated(Vector2I size)
        {
            // create a local copy of this box
            Box2I box = this;
            box.Inflate(size);
            return box;
        }

        /// <summary>
        /// Extend this Box2i to encapsulate a given point.
        /// </summary>
        /// <param name="point">The point to contain.</param>
        public void Extend(Vector2I point)
        {
            _min = Vector2I.ComponentMin(_min, point);
            _max = Vector2I.ComponentMax(_max, point);
        }

        /// <summary>
        /// Extend this Box2i to encapsulate a given point.
        /// </summary>
        /// <param name="point">The point to contain.</param>
        /// <returns>The inflated box.</returns>
        [Pure]
        public readonly Box2I Extended(Vector2I point)
        {
            // create a local copy of this box
            Box2I box = this;
            box.Extend(point);
            return box;
        }

        /// <summary>
        /// Equality comparator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        public static bool operator ==(Box2I left, Box2I right) => left.Equals(right);

        /// <summary>
        /// Inequality comparator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        public static bool operator !=(Box2I left, Box2I right) => !(left == right);

        /// <inheritdoc/>
        public override readonly bool Equals(object? obj) => obj is Box2I box && Equals(box);

        /// <inheritdoc/>
        public readonly bool Equals(Box2I other) => _min.Equals(other._min) &&
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
