﻿using SatisfactorySaveNet.Abstracts.Maths.Vector;
using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace SatisfactorySaveNet.Abstracts.Maths.Matrix
{
    /// <summary>
    /// Represents a 2x3 matrix.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix2X3 : IEquatable<Matrix2X3>, IFormattable
    {
        /// <summary>
        /// Top row of the matrix.
        /// </summary>
        public Vector.Vector3 Row0;

        /// <summary>
        /// Bottom row of the matrix.
        /// </summary>
        public Vector.Vector3 Row1;

        /// <summary>
        /// The zero matrix.
        /// </summary>
        public static readonly Matrix2X3 Zero = new(Vector.Vector3.Zero, Vector.Vector3.Zero);

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix2X3"/> struct.
        /// </summary>
        /// <param name="row0">Top row of the matrix.</param>
        /// <param name="row1">Bottom row of the matrix.</param>
        public Matrix2X3(Vector.Vector3 row0, Vector.Vector3 row1)
        {
            Row0 = row0;
            Row1 = row1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix2X3"/> struct.
        /// </summary>
        /// <param name="m00">First item of the first row of the matrix.</param>
        /// <param name="m01">Second item of the first row of the matrix.</param>
        /// <param name="m02">Third item of the first row of the matrix.</param>
        /// <param name="m10">First item of the second row of the matrix.</param>
        /// <param name="m11">Second item of the second row of the matrix.</param>
        /// <param name="m12">Third item of the second row of the matrix.</param>
        public Matrix2X3
        (
            float m00, float m01, float m02,
            float m10, float m11, float m12
        )
        {
            Row0 = new Vector.Vector3(m00, m01, m02);
            Row1 = new Vector.Vector3(m10, m11, m12);
        }

        /// <summary>
        /// Gets or sets the first column of this matrix.
        /// </summary>
        public Vector2 Column0
        {
            readonly get => new(Row0.X, Row1.X);
            set
            {
                Row0.X = value.X;
                Row1.X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the second column of this matrix.
        /// </summary>
        public Vector2 Column1
        {
            readonly get => new(Row0.Y, Row1.Y);
            set
            {
                Row0.Y = value.X;
                Row1.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the third column of this matrix.
        /// </summary>
        public Vector2 Column2
        {
            readonly get => new(Row0.Z, Row1.Z);
            set
            {
                Row0.Z = value.X;
                Row1.Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the value at row 1, column 1 of this instance.
        /// </summary>
        public float M11
        {
            readonly get => Row0.X;
            set => Row0.X = value;
        }

        /// <summary>
        /// Gets or sets the value at row 1, column 2 of this instance.
        /// </summary>
        public float M12
        {
            readonly get => Row0.Y;
            set => Row0.Y = value;
        }

        /// <summary>
        /// Gets or sets the value at row 1, column 3 of this instance.
        /// </summary>
        public float M13
        {
            readonly get => Row0.Z;
            set => Row0.Z = value;
        }

        /// <summary>
        /// Gets or sets the value at row 2, column 1 of this instance.
        /// </summary>
        public float M21
        {
            readonly get => Row1.X;
            set => Row1.X = value;
        }

        /// <summary>
        /// Gets or sets the value at row 2, column 2 of this instance.
        /// </summary>
        public float M22
        {
            readonly get => Row1.Y;
            set => Row1.Y = value;
        }

        /// <summary>
        /// Gets or sets the value at row 2, column 3 of this instance.
        /// </summary>
        public float M23
        {
            readonly get => Row1.Z;
            set => Row1.Z = value;
        }

        /// <summary>
        /// Gets or sets the values along the main diagonal of the matrix.
        /// </summary>
        public Vector2 Diagonal
        {
            readonly get => new(Row0.X, Row1.Y);
            set
            {
                Row0.X = value.X;
                Row1.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets the trace of the matrix, the sum of the values along the diagonal.
        /// </summary>
        public readonly float Trace => Row0.X + Row1.Y;

        /// <summary>
        /// Gets or sets the value at a specified row and column.
        /// </summary>
        /// <param name="rowIndex">The index of the row.</param>
        /// <param name="columnIndex">The index of the column.</param>
        /// <returns>The element at the given row and column index.</returns>
        public float this[int rowIndex, int columnIndex]
        {
            readonly get
            {
                var tmp = rowIndex == 0
                    ? Row0[columnIndex]
                    : rowIndex;
                return tmp == 1
                    ? Row1[columnIndex]
                    : throw new IndexOutOfRangeException("You tried to access this matrix at: (" + rowIndex + ", " +
                                                   columnIndex + ")");
            }

            set
            {
                if (rowIndex == 0)
                {
                    Row0[columnIndex] = value;
                }
                else
                {
                    Row1[columnIndex] = rowIndex == 1
                        ? value
                        : throw new IndexOutOfRangeException("You tried to set this matrix at: (" + rowIndex + ", " +
                                                                           columnIndex + ")");
                }
            }
        }

        /// <summary>
        /// Builds a rotation matrix.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix2x3 instance.</param>
        public static void CreateRotation(float angle, out Matrix2X3 result)
        {
            var cos = MathF.Cos(angle);
            var sin = MathF.Sin(angle);

            result.Row0.X = cos;
            result.Row0.Y = sin;
            result.Row0.Z = 0;
            result.Row1.X = -sin;
            result.Row1.Y = cos;
            result.Row1.Z = 0;
        }

        /// <summary>
        /// Builds a rotation matrix.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix2x3 instance.</returns>
        [Pure]
        public static Matrix2X3 CreateRotation(float angle)
        {
            CreateRotation(angle, out var result);
            return result;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="scale">Single scale factor for the x, y, and z axes.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(float scale, out Matrix2X3 result)
        {
            result.Row0.X = scale;
            result.Row0.Y = 0;
            result.Row0.Z = 0;
            result.Row1.X = 0;
            result.Row1.Y = scale;
            result.Row1.Z = 0;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="scale">Single scale factor for the x and y axes.</param>
        /// <returns>A scale matrix.</returns>
        [Pure]
        public static Matrix2X3 CreateScale(float scale)
        {
            CreateScale(scale, out var result);
            return result;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="scale">Scale factors for the x and y axes.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(Vector2 scale, out Matrix2X3 result)
        {
            result.Row0.X = scale.X;
            result.Row0.Y = 0;
            result.Row0.Z = 0;
            result.Row1.X = 0;
            result.Row1.Y = scale.Y;
            result.Row1.Z = 0;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="scale">Scale factors for the x and y axes.</param>
        /// <returns>A scale matrix.</returns>
        [Pure]
        public static Matrix2X3 CreateScale(Vector2 scale)
        {
            CreateScale(scale, out var result);
            return result;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="x">Scale factor for the x axis.</param>
        /// <param name="y">Scale factor for the y axis.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(float x, float y, out Matrix2X3 result)
        {
            result.Row0.X = x;
            result.Row0.Y = 0;
            result.Row0.Z = 0;
            result.Row1.X = 0;
            result.Row1.Y = y;
            result.Row1.Z = 0;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="x">Scale factor for the x axis.</param>
        /// <param name="y">Scale factor for the y axis.</param>
        /// <returns>A scale matrix.</returns>
        [Pure]
        public static Matrix2X3 CreateScale(float x, float y)
        {
            CreateScale(x, y, out var result);
            return result;
        }

        /// <summary>
        /// Multiplies and instance by a scalar.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        public static void Mult(in Matrix2X3 left, float right, out Matrix2X3 result)
        {
            result.Row0.X = left.Row0.X * right;
            result.Row0.Y = left.Row0.Y * right;
            result.Row0.Z = left.Row0.Z * right;
            result.Row1.X = left.Row1.X * right;
            result.Row1.Y = left.Row1.Y * right;
            result.Row1.Z = left.Row1.Z * right;
        }

        /// <summary>
        /// Multiplies and instance by a scalar.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        [Pure]
        public static Matrix2X3 Mult(Matrix2X3 left, float right)
        {
            Mult(in left, right, out var result);
            return result;
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        public static void Mult(in Matrix2X3 left, in Matrix3X2 right, out Matrix2 result)
        {
            var leftM11 = left.Row0.X;
            var leftM12 = left.Row0.Y;
            var leftM13 = left.Row0.Z;
            var leftM21 = left.Row1.X;
            var leftM22 = left.Row1.Y;
            var leftM23 = left.Row1.Z;
            var rightM11 = right.Row0.X;
            var rightM12 = right.Row0.Y;
            var rightM21 = right.Row1.X;
            var rightM22 = right.Row1.Y;
            var rightM31 = right.Row2.X;
            var rightM32 = right.Row2.Y;

            result.Row0.X = (leftM11 * rightM11) + (leftM12 * rightM21) + (leftM13 * rightM31);
            result.Row0.Y = (leftM11 * rightM12) + (leftM12 * rightM22) + (leftM13 * rightM32);
            result.Row1.X = (leftM21 * rightM11) + (leftM22 * rightM21) + (leftM23 * rightM31);
            result.Row1.Y = (leftM21 * rightM12) + (leftM22 * rightM22) + (leftM23 * rightM32);
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        [Pure]
        public static Matrix2 Mult(Matrix2X3 left, Matrix3X2 right)
        {
            Mult(in left, in right, out var result);
            return result;
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        public static void Mult(in Matrix2X3 left, in Matrix3 right, out Matrix2X3 result)
        {
            var leftM11 = left.Row0.X;
            var leftM12 = left.Row0.Y;
            var leftM13 = left.Row0.Z;
            var leftM21 = left.Row1.X;
            var leftM22 = left.Row1.Y;
            var leftM23 = left.Row1.Z;
            var rightM11 = right.Row0.X;
            var rightM12 = right.Row0.Y;
            var rightM13 = right.Row0.Z;
            var rightM21 = right.Row1.X;
            var rightM22 = right.Row1.Y;
            var rightM23 = right.Row1.Z;
            var rightM31 = right.Row2.X;
            var rightM32 = right.Row2.Y;
            var rightM33 = right.Row2.Z;

            result.Row0.X = (leftM11 * rightM11) + (leftM12 * rightM21) + (leftM13 * rightM31);
            result.Row0.Y = (leftM11 * rightM12) + (leftM12 * rightM22) + (leftM13 * rightM32);
            result.Row0.Z = (leftM11 * rightM13) + (leftM12 * rightM23) + (leftM13 * rightM33);
            result.Row1.X = (leftM21 * rightM11) + (leftM22 * rightM21) + (leftM23 * rightM31);
            result.Row1.Y = (leftM21 * rightM12) + (leftM22 * rightM22) + (leftM23 * rightM32);
            result.Row1.Z = (leftM21 * rightM13) + (leftM22 * rightM23) + (leftM23 * rightM33);
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        [Pure]
        public static Matrix2X3 Mult(Matrix2X3 left, Matrix3 right)
        {
            Mult(in left, in right, out var result);
            return result;
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        public static void Mult(in Matrix2X3 left, in Matrix3X4 right, out Matrix2X4 result)
        {
            var leftM11 = left.Row0.X;
            var leftM12 = left.Row0.Y;
            var leftM13 = left.Row0.Z;
            var leftM21 = left.Row1.X;
            var leftM22 = left.Row1.Y;
            var leftM23 = left.Row1.Z;
            var rightM11 = right.Row0.X;
            var rightM12 = right.Row0.Y;
            var rightM13 = right.Row0.Z;
            var rightM14 = right.Row0.W;
            var rightM21 = right.Row1.X;
            var rightM22 = right.Row1.Y;
            var rightM23 = right.Row1.Z;
            var rightM24 = right.Row1.W;
            var rightM31 = right.Row2.X;
            var rightm32 = right.Row2.Y;
            var rightM33 = right.Row2.Z;
            var rightM34 = right.Row2.W;

            result.Row0.X = (leftM11 * rightM11) + (leftM12 * rightM21) + (leftM13 * rightM31);
            result.Row0.Y = (leftM11 * rightM12) + (leftM12 * rightM22) + (leftM13 * rightm32);
            result.Row0.Z = (leftM11 * rightM13) + (leftM12 * rightM23) + (leftM13 * rightM33);
            result.Row0.W = (leftM11 * rightM14) + (leftM12 * rightM24) + (leftM13 * rightM34);
            result.Row1.X = (leftM21 * rightM11) + (leftM22 * rightM21) + (leftM23 * rightM31);
            result.Row1.Y = (leftM21 * rightM12) + (leftM22 * rightM22) + (leftM23 * rightm32);
            result.Row1.Z = (leftM21 * rightM13) + (leftM22 * rightM23) + (leftM23 * rightM33);
            result.Row1.W = (leftM21 * rightM14) + (leftM22 * rightM24) + (leftM23 * rightM34);
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        public static Matrix2X4 Mult(Matrix2X3 left, Matrix3X4 right)
        {
            Mult(in left, in right, out var result);
            return result;
        }

        /// <summary>
        /// Adds two instances.
        /// </summary>
        /// <param name="left">The left operand of the addition.</param>
        /// <param name="right">The right operand of the addition.</param>
        /// <param name="result">A new instance that is the result of the addition.</param>
        public static void Add(in Matrix2X3 left, in Matrix2X3 right, out Matrix2X3 result)
        {
            result.Row0.X = left.Row0.X + right.Row0.X;
            result.Row0.Y = left.Row0.Y + right.Row0.Y;
            result.Row0.Z = left.Row0.Z + right.Row0.Z;
            result.Row1.X = left.Row1.X + right.Row1.X;
            result.Row1.Y = left.Row1.Y + right.Row1.Y;
            result.Row1.Z = left.Row1.Z + right.Row1.Z;
        }

        /// <summary>
        /// Adds two instances.
        /// </summary>
        /// <param name="left">The left operand of the addition.</param>
        /// <param name="right">The right operand of the addition.</param>
        /// <returns>A new instance that is the result of the addition.</returns>
        [Pure]
        public static Matrix2X3 Add(Matrix2X3 left, Matrix2X3 right)
        {
            Add(in left, in right, out var result);
            return result;
        }

        /// <summary>
        /// Subtracts two instances.
        /// </summary>
        /// <param name="left">The left operand of the subtraction.</param>
        /// <param name="right">The right operand of the subtraction.</param>
        /// <param name="result">A new instance that is the result of the subtraction.</param>
        public static void Subtract(in Matrix2X3 left, in Matrix2X3 right, out Matrix2X3 result)
        {
            result.Row0.X = left.Row0.X - right.Row0.X;
            result.Row0.Y = left.Row0.Y - right.Row0.Y;
            result.Row0.Z = left.Row0.Z - right.Row0.Z;
            result.Row1.X = left.Row1.X - right.Row1.X;
            result.Row1.Y = left.Row1.Y - right.Row1.Y;
            result.Row1.Z = left.Row1.Z - right.Row1.Z;
        }

        /// <summary>
        /// Subtracts two instances.
        /// </summary>
        /// <param name="left">The left operand of the subtraction.</param>
        /// <param name="right">The right operand of the subtraction.</param>
        /// <returns>A new instance that is the result of the subtraction.</returns>
        [Pure]
        public static Matrix2X3 Subtract(Matrix2X3 left, Matrix2X3 right)
        {
            Subtract(in left, in right, out var result);
            return result;
        }

        /// <summary>
        /// Calculate the transpose of the given matrix.
        /// </summary>
        /// <param name="mat">The matrix to transpose.</param>
        /// <param name="result">The transpose of the given matrix.</param>
        public static void Transpose(in Matrix2X3 mat, out Matrix3X2 result)
        {
            result.Row0.X = mat.Row0.X;
            result.Row0.Y = mat.Row1.X;
            result.Row1.X = mat.Row0.Y;
            result.Row1.Y = mat.Row1.Y;
            result.Row2.X = mat.Row0.Z;
            result.Row2.Y = mat.Row1.Z;
        }

        /// <summary>
        /// Calculate the transpose of the given matrix.
        /// </summary>
        /// <param name="mat">The matrix to transpose.</param>
        /// <returns>The transpose of the given matrix.</returns>
        [Pure]
        public static Matrix3X2 Transpose(Matrix2X3 mat)
        {
            Transpose(in mat, out var result);
            return result;
        }

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2x3 which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix2X3 operator *(float left, Matrix2X3 right)
        {
            return Mult(right, left);
        }

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2x3 which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix2X3 operator *(Matrix2X3 left, float right)
        {
            return Mult(left, right);
        }

        /// <summary>
        /// Matrix multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2 which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix2 operator *(Matrix2X3 left, Matrix3X2 right)
        {
            return Mult(left, right);
        }

        /// <summary>
        /// Matrix multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2x3 which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix2X3 operator *(Matrix2X3 left, Matrix3 right)
        {
            return Mult(left, right);
        }

        /// <summary>
        /// Matrix multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2x4 which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix2X4 operator *(Matrix2X3 left, Matrix3X4 right)
        {
            return Mult(left, right);
        }

        /// <summary>
        /// Matrix addition.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2x3 which holds the result of the addition.</returns>
        [Pure]
        public static Matrix2X3 operator +(Matrix2X3 left, Matrix2X3 right)
        {
            return Add(left, right);
        }

        /// <summary>
        /// Matrix subtraction.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2x3 which holds the result of the subtraction.</returns>
        [Pure]
        public static Matrix2X3 operator -(Matrix2X3 left, Matrix2X3 right)
        {
            return Subtract(left, right);
        }

        /// <summary>
        /// Compares two instances for equality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        [Pure]
        public static bool operator ==(Matrix2X3 left, Matrix2X3 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two instances for inequality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equal right; false otherwise.</returns>
        [Pure]
        public static bool operator !=(Matrix2X3 left, Matrix2X3 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a System.String that represents the current Matrix2x3.
        /// </summary>
        /// <returns>The string representation of the matrix.</returns>
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
            var row0 = Row0.ToString(format, formatProvider);
            var row1 = Row1.ToString(format, formatProvider);
            return $"{row0}\n{row1}";
        }

        /// <summary>
        /// Returns the hashcode for this instance.
        /// </summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
        public readonly override int GetHashCode()
        {
            return HashCode.Combine(Row0, Row1);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare tresult.</param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        [Pure]
        public readonly override bool Equals(object? obj)
        {
            return obj is Matrix2X3 matrix && Equals(matrix);
        }

        /// <summary>
        /// Indicates whether the current matrix is equal to another matrix.
        /// </summary>
        /// <param name="other">An matrix to compare with this matrix.</param>
        /// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
        [Pure]
        public readonly bool Equals(Matrix2X3 other)
        {
            return Row0 == other.Row0 &&
                Row1 == other.Row1;
        }
    }
}
