using SatisfactorySaveNet.Abstracts.Maths.Vector;
using System.Diagnostics.Contracts;

namespace SatisfactorySaveNet.Abstracts.Maths.Geometry
{
    /// <summary>
    /// Represents a cubic bezier curve with two anchor and two control points.
    /// </summary>
    [Serializable]
    public struct BezierCurveCubic
    {
        /// <summary>
        /// Start anchor point.
        /// </summary>
        private Vector2 _startAnchor;

        /// <summary>
        /// End anchor point.
        /// </summary>
        private Vector2 _endAnchor;

        /// <summary>
        /// First control point, controls the direction of the curve start.
        /// </summary>
        private Vector2 _firstControlPoint;

        /// <summary>
        /// Second control point, controls the direction of the curve end.
        /// </summary>
        private Vector2 _secondControlPoint;

        /// <summary>
        /// Gets or sets the parallel value.
        /// </summary>
        /// <remarks>
        /// This value defines whether the curve should be calculated as a
        /// parallel curve to the original bezier curve. A value of 0.0f represents
        /// the original curve, 5.0f i.e. stands for a curve that has always a distance
        /// of 5.f to the orignal curve at any point.
        /// </remarks>
        private float _parallel;

        public Vector2 StartAnchor
        {
            readonly get => _startAnchor;
            set => _startAnchor = value;
        }

        public Vector2 EndAnchor
        {
            readonly get => _endAnchor;
            set => _endAnchor = value;
        }

        public Vector2 FirstControlPoint
        {
            readonly get => _firstControlPoint;
            set => _firstControlPoint = value;
        }

        public Vector2 SecondControlPoint
        {
            readonly get => _secondControlPoint;
            set => _secondControlPoint = value;
        }

        public float Parallel
        {
            readonly get => _parallel;
            set => _parallel = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BezierCurveCubic"/> struct.
        /// </summary>
        /// <param name="startAnchor">The start anchor point.</param>
        /// <param name="endAnchor">The end anchor point.</param>
        /// <param name="firstControlPoint">The first control point.</param>
        /// <param name="secondControlPoint">The second control point.</param>
        public BezierCurveCubic
        (
            Vector2 startAnchor,
            Vector2 endAnchor,
            Vector2 firstControlPoint,
            Vector2 secondControlPoint
        )
        {
            StartAnchor = startAnchor;
            EndAnchor = endAnchor;
            FirstControlPoint = firstControlPoint;
            SecondControlPoint = secondControlPoint;
            Parallel = 0.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BezierCurveCubic"/> struct.
        /// </summary>
        /// <param name="parallel">The parallel value.</param>
        /// <param name="startAnchor">The start anchor point.</param>
        /// <param name="endAnchor">The end anchor point.</param>
        /// <param name="firstControlPoint">The first control point.</param>
        /// <param name="secondControlPoint">The second control point.</param>
        public BezierCurveCubic
        (
            float parallel,
            Vector2 startAnchor,
            Vector2 endAnchor,
            Vector2 firstControlPoint,
            Vector2 secondControlPoint
        )
        {
            Parallel = parallel;
            StartAnchor = startAnchor;
            EndAnchor = endAnchor;
            FirstControlPoint = firstControlPoint;
            SecondControlPoint = secondControlPoint;
        }

        /// <summary>
        /// Calculates the point with the specified t.
        /// </summary>
        /// <param name="t">The t value, between 0.0f and 1.0f.</param>
        /// <returns>Resulting point.</returns>
        [Pure]
        public readonly Vector2 CalculatePoint(float t)
        {
            float c = 1.0f - t;

            float x = (StartAnchor.X * c * c * c) + (FirstControlPoint.X * 3 * t * c * c) +
                (SecondControlPoint.X * 3 * t * t * c) + (EndAnchor.X * t * t * t);

            float y = (StartAnchor.Y * c * c * c) + (FirstControlPoint.Y * 3 * t * c * c) +
                (SecondControlPoint.Y * 3 * t * t * c) + (EndAnchor.Y * t * t * t);

            Vector2 r = new(x, y);

            if (Parallel == 0.0f)
            {
                return r;
            }

            Vector2 perpendicular = t == 0.0f ? FirstControlPoint - StartAnchor : r - CalculatePointOfDerivative(t);
            return r + (Vector2.Normalize(perpendicular).PerpendicularRight * Parallel);
        }

        /// <summary>
        /// Calculates the point with the specified t of the derivative of this function.
        /// </summary>
        /// <param name="t">The t, value between 0.0f and 1.0f.</param>
        /// <returns>Resulting point.</returns>
        [Pure]
        private readonly Vector2 CalculatePointOfDerivative(float t)
        {
            float c = 1.0f - t;
            Vector2 r = new(
                (c * c * StartAnchor.X) + (2 * t * c * FirstControlPoint.X) + (t * t * SecondControlPoint.X),
                (c * c * StartAnchor.Y) + (2 * t * c * FirstControlPoint.Y) + (t * t * SecondControlPoint.Y)
            );

            return r;
        }

        /// <summary>
        /// Calculates the length of this bezier curve.
        /// </summary>
        /// <param name="precision">The precision.</param>
        /// <returns>Length of the curve.</returns>
        /// <remarks>
        /// The precision gets better when the <paramref name="precision"/>
        /// value gets smaller.
        /// </remarks>
        [Pure]
        public readonly float CalculateLength(float precision)
        {
            float length = 0.0f;
            Vector2 old = CalculatePoint(0.0f);

            for (float i = precision; i < 1.0f + precision; i += precision)
            {
                Vector2 n = CalculatePoint(i);
                length += (n - old).Length;
                old = n;
            }

            return length;
        }
    }
}
