using SatisfactorySaveNet.Abstracts.Maths.Vector;
using System;
using System.Diagnostics.Contracts;

namespace SatisfactorySaveNet.Abstracts.Maths.Geometry
{
    /// <summary>
    /// Represents a quadric bezier curve with two anchor and one control point.
    /// </summary>
    [Serializable]
    public struct BezierCurveQuadric
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
        /// Control point, controls the direction of both endings of the curve.
        /// </summary>
        private Vector2 _controlPoint;

        /// <summary>
        /// The parallel value.
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

        public Vector2 ControlPoint
        {
            readonly get => _controlPoint;
            set => _controlPoint = value;
        }

        public float Parallel
        {
            readonly get => _parallel;
            set => _parallel = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BezierCurveQuadric"/> struct.
        /// </summary>
        /// <param name="startAnchor">The start anchor.</param>
        /// <param name="endAnchor">The end anchor.</param>
        /// <param name="controlPoint">The control point.</param>
        public BezierCurveQuadric(Vector2 startAnchor, Vector2 endAnchor, Vector2 controlPoint)
        {
            StartAnchor = startAnchor;
            EndAnchor = endAnchor;
            ControlPoint = controlPoint;
            Parallel = 0.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BezierCurveQuadric"/> struct.
        /// </summary>
        /// <param name="parallel">The parallel value.</param>
        /// <param name="startAnchor">The start anchor.</param>
        /// <param name="endAnchor">The end anchor.</param>
        /// <param name="controlPoint">The control point.</param>
        public BezierCurveQuadric(float parallel, Vector2 startAnchor, Vector2 endAnchor, Vector2 controlPoint)
        {
            Parallel = parallel;
            StartAnchor = startAnchor;
            EndAnchor = endAnchor;
            ControlPoint = controlPoint;
        }

        /// <summary>
        /// Calculates the point with the specified t.
        /// </summary>
        /// <param name="t">The t value, between 0.0f and 1.0f.</param>
        /// <returns>Resulting point.</returns>
        [Pure]
        public Vector2 CalculatePoint(float t)
        {
            var c = 1.0f - t;
            Vector2 r = new(
                (c * c * StartAnchor.X) + (2 * t * c * ControlPoint.X) + (t * t * EndAnchor.X),
                (c * c * StartAnchor.Y) + (2 * t * c * ControlPoint.Y) + (t * t * EndAnchor.Y)
            );

            if (Parallel == 0.0f)
            {
                return r;
            }

            var perpendicular = t == 0.0f ? ControlPoint - StartAnchor : r - CalculatePointOfDerivative(t);
            return r + (Vector2.Normalize(perpendicular).PerpendicularRight * Parallel);
        }

        /// <summary>
        /// Calculates the point with the specified t of the derivative of this function.
        /// </summary>
        /// <param name="t">The t, value between 0.0f and 1.0f.</param>
        /// <returns>Resulting point.</returns>
        [Pure]
        private Vector2 CalculatePointOfDerivative(float t)
        {
            Vector2 r = new()
            {
                X = ((1.0f - t) * StartAnchor.X) + (t * ControlPoint.X),
                Y = ((1.0f - t) * StartAnchor.Y) + (t * ControlPoint.Y)
            };

            return r;
        }

        /// <summary>
        /// Calculates the length of this bezier curve.
        /// </summary>
        /// <param name="precision">The precision.</param>
        /// <returns>Length of curve.</returns>
        /// <remarks>
        /// The precision gets better when the <paramref name="precision"/>
        /// value gets smaller.
        /// </remarks>
        [Pure]
        public float CalculateLength(float precision)
        {
            var length = 0.0f;
            var old = CalculatePoint(0.0f);

            for (var i = precision; i < 1.0f + precision; i += precision)
            {
                var n = CalculatePoint(i);
                length += (n - old).Length;
                old = n;
            }

            return length;
        }
    }
}
