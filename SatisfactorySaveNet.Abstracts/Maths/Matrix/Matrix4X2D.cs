using SatisfactorySaveNet.Abstracts.Maths.Vector;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace SatisfactorySaveNet.Abstracts.Maths.Matrix
{
    /// <summary>
    /// Represents a 4x2 matrix.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4X2D : IEquatable<Matrix4X2D>, IFormattable
    {
        /// <summary>
        /// Top row of the matrix.
        /// </summary>
        public Vector2D Row0;

        /// <summary>
        /// Second row of the matrix.
        /// </summary>
        public Vector2D Row1;

        /// <summary>
        /// Third row of the matrix.
        /// </summary>
        public Vector2D Row2;

        /// <summary>
        /// Bottom row of the matrix.
        /// </summary>
        public Vector2D Row3;

        /// <summary>
        /// The zero matrix.
        /// </summary>
        public static readonly Matrix4X2D Zero = new(
            Vector2D.Zero,
            Vector2D.Zero,
            Vector2D.Zero,
            Vector2D.Zero
        );

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix4X2D"/> struct.
        /// </summary>
        /// <param name="row0">Top row of the matrix.</param>
        /// <param name="row1">Second row of the matrix.</param>
        /// <param name="row2">Third row of the matrix.</param>
        /// <param name="row3">Bottom row of the matrix.</param>
        public Matrix4X2D(Vector2D row0, Vector2D row1, Vector2D row2, Vector2D row3)
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix4X2D"/> struct.
        /// </summary>
        /// <param name="m00">First item of the first row of the matrix.</param>
        /// <param name="m01">Second item of the first row of the matrix.</param>
        /// <param name="m10">First item of the second row of the matrix.</param>
        /// <param name="m11">Second item of the second row of the matrix.</param>
        /// <param name="m20">First item of the third row of the matrix.</param>
        /// <param name="m21">Second item of the third row of the matrix.</param>
        /// <param name="m30">First item of the fourth row of the matrix.</param>
        /// <param name="m31">Second item of the fourth row of the matrix.</param>
        public Matrix4X2D
        (
            double m00, double m01,
            double m10, double m11,
            double m20, double m21,
            double m30, double m31
        )
        {
            Row0 = new Vector2D(m00, m01);
            Row1 = new Vector2D(m10, m11);
            Row2 = new Vector2D(m20, m21);
            Row3 = new Vector2D(m30, m31);
        }

        /// <summary>
        /// Gets or sets the first column of this matrix.
        /// </summary>
        public Vector4D Column0
        {
            readonly get => new(Row0.X, Row1.X, Row2.X, Row3.X);
            set
            {
                Row0.X = value.X;
                Row1.X = value.Y;
                Row2.X = value.Z;
                Row3.X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets the second column of this matrix.
        /// </summary>
        public Vector4D Column1
        {
            readonly get => new(Row0.Y, Row1.Y, Row2.Y, Row3.X);
            set
            {
                Row0.Y = value.X;
                Row1.Y = value.Y;
                Row2.Y = value.Z;
                Row3.Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets the value at row 1, column 1 of this instance.
        /// </summary>
        public double M11
        {
            readonly get => Row0.X;
            set => Row0.X = value;
        }

        /// <summary>
        /// Gets or sets the value at row 1, column 2 of this instance.
        /// </summary>
        public double M12
        {
            readonly get => Row0.Y;
            set => Row0.Y = value;
        }

        /// <summary>
        /// Gets or sets the value at row 2, column 1 of this instance.
        /// </summary>
        public double M21
        {
            readonly get => Row1.X;
            set => Row1.X = value;
        }

        /// <summary>
        /// Gets or sets the value at row 2, column 2 of this instance.
        /// </summary>
        public double M22
        {
            readonly get => Row1.Y;
            set => Row1.Y = value;
        }

        /// <summary>
        /// Gets or sets the value at row 3, column 1 of this instance.
        /// </summary>
        public double M31
        {
            readonly get => Row2.X;
            set => Row2.X = value;
        }

        /// <summary>
        /// Gets or sets the value at row 3, column 2 of this instance.
        /// </summary>
        public double M32
        {
            readonly get => Row2.Y;
            set => Row2.Y = value;
        }

        /// <summary>
        /// Gets or sets the value at row 4, column 1 of this instance.
        /// </summary>
        public double M41
        {
            readonly get => Row3.X;
            set => Row3.X = value;
        }

        /// <summary>
        /// Gets or sets the value at row 4, column 2 of this instance.
        /// </summary>
        public double M42
        {
            readonly get => Row3.Y;
            set => Row3.Y = value;
        }

        /// <summary>
        /// Gets or sets the values along the main diagonal of the matrix.
        /// </summary>
        public Vector2D Diagonal
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
        public readonly double Trace => Row0.X + Row1.Y;

        /// <summary>
        /// Gets or sets the value at a specified row and column.
        /// </summary>
        /// <param name="rowIndex">The index of the row.</param>
        /// <param name="columnIndex">The index of the column.</param>
        /// <returns>The element at the given row and column index.</returns>
        public double this[int rowIndex, int columnIndex]
        {
            readonly get
            {
                if (rowIndex == 0)
                {
                    return Row0[columnIndex];
                }

                if (rowIndex == 1)
                {
                    return Row1[columnIndex];
                }

                var tmp = rowIndex == 2
                    ? Row2[columnIndex]
                    : rowIndex;

#pragma warning disable S112 // General or reserved exceptions should never be thrown
                return tmp == 3
                    ? Row3[columnIndex]
                    : throw new IndexOutOfRangeException("You tried to access this matrix at: (" + rowIndex + ", " +
                                                   columnIndex + ")");
#pragma warning restore S112 // General or reserved exceptions should never be thrown
            }
            set
            {
                if (rowIndex == 0)
                {
                    Row0[columnIndex] = value;
                }
                else if (rowIndex == 1)
                {
                    Row1[columnIndex] = value;
                }
                else if (rowIndex == 2)
                {
                    Row2[columnIndex] = value;
                }
                else
                {
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                    Row3[columnIndex] = rowIndex == 3
                        ? value
                        : throw new IndexOutOfRangeException("You tried to set this matrix at: (" + rowIndex + ", " +
                                                                           columnIndex + ")");
#pragma warning restore S112 // General or reserved exceptions should never be thrown
                }
            }
        }

        /// <summary>
        /// Builds a rotation matrix.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix3x2 instance.</param>
        public static void CreateRotation(double angle, out Matrix4X2D result)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            result.Row0.X = cos;
            result.Row0.Y = sin;
            result.Row1.X = -sin;
            result.Row1.Y = cos;
            result.Row2.X = 0;
            result.Row2.Y = 0;
            result.Row3.X = 0;
            result.Row3.Y = 0;
        }

        /// <summary>
        /// Builds a rotation matrix.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix3x2 instance.</returns>
        [Pure]
        public static Matrix4X2D CreateRotation(double angle)
        {
            CreateRotation(angle, out Matrix4X2D result);
            return result;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="scale">Single scale factor for the x, y, and z axes.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(double scale, out Matrix4X2D result)
        {
            result.Row0.X = scale;
            result.Row0.Y = 0;
            result.Row1.X = 0;
            result.Row1.Y = scale;
            result.Row2.X = 0;
            result.Row2.Y = 0;
            result.Row3.X = 0;
            result.Row3.Y = 0;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="scale">Single scale factor for the x and y axes.</param>
        /// <returns>A scale matrix.</returns>
        [Pure]
        public static Matrix4X2D CreateScale(double scale)
        {
            CreateScale(scale, out Matrix4X2D result);
            return result;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="scale">Scale factors for the x and y axes.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(Vector2D scale, out Matrix4X2D result)
        {
            result.Row0.X = scale.X;
            result.Row0.Y = 0;
            result.Row1.X = 0;
            result.Row1.Y = scale.Y;
            result.Row2.X = 0;
            result.Row2.Y = 0;
            result.Row3.X = 0;
            result.Row3.Y = 0;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="scale">Scale factors for the x and y axes.</param>
        /// <returns>A scale matrix.</returns>
        [Pure]
        public static Matrix4X2D CreateScale(Vector2D scale)
        {
            CreateScale(scale, out Matrix4X2D result);
            return result;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="x">Scale factor for the x axis.</param>
        /// <param name="y">Scale factor for the y axis.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(double x, double y, out Matrix4X2D result)
        {
            result.Row0.X = x;
            result.Row0.Y = 0;
            result.Row1.X = 0;
            result.Row1.Y = y;
            result.Row2.X = 0;
            result.Row2.Y = 0;
            result.Row3.X = 0;
            result.Row3.Y = 0;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="x">Scale factor for the x axis.</param>
        /// <param name="y">Scale factor for the y axis.</param>
        /// <returns>A scale matrix.</returns>
        [Pure]
        public static Matrix4X2D CreateScale(double x, double y)
        {
            CreateScale(x, y, out Matrix4X2D result);
            return result;
        }

        /// <summary>
        /// Multiplies and instance by a scalar.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        public static void Mult(in Matrix4X2D left, double right, out Matrix4X2D result)
        {
            result.Row0.X = left.Row0.X * right;
            result.Row0.Y = left.Row0.Y * right;
            result.Row1.X = left.Row1.X * right;
            result.Row1.Y = left.Row1.Y * right;
            result.Row2.X = left.Row2.X * right;
            result.Row2.Y = left.Row2.Y * right;
            result.Row3.X = left.Row3.X * right;
            result.Row3.Y = left.Row3.Y * right;
        }

        /// <summary>
        /// Multiplies and instance by a scalar.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        [Pure]
        public static Matrix4X2D Mult(Matrix4X2D left, double right)
        {
            Mult(in left, right, out Matrix4X2D result);
            return result;
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        public static void Mult(in Matrix4X2D left, in Matrix2D right, out Matrix4X2D result)
        {
            double leftM11 = left.Row0.X;
            double leftM12 = left.Row0.Y;
            double leftM21 = left.Row1.X;
            double leftM22 = left.Row1.Y;
            double leftM31 = left.Row2.X;
            double leftM32 = left.Row2.Y;
            double leftM41 = left.Row3.X;
            double leftM42 = left.Row3.Y;
            double rightM11 = right.Row0.X;
            double rightM12 = right.Row0.Y;
            double rightM21 = right.Row1.X;
            double rightM22 = right.Row1.Y;

            result.Row0.X = (leftM11 * rightM11) + (leftM12 * rightM21);
            result.Row0.Y = (leftM11 * rightM12) + (leftM12 * rightM22);
            result.Row1.X = (leftM21 * rightM11) + (leftM22 * rightM21);
            result.Row1.Y = (leftM21 * rightM12) + (leftM22 * rightM22);
            result.Row2.X = (leftM31 * rightM11) + (leftM32 * rightM21);
            result.Row2.Y = (leftM31 * rightM12) + (leftM32 * rightM22);
            result.Row3.X = (leftM41 * rightM11) + (leftM42 * rightM21);
            result.Row3.Y = (leftM41 * rightM12) + (leftM42 * rightM22);
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        [Pure]
        public static Matrix4X2D Mult(Matrix4X2D left, Matrix2D right)
        {
            Mult(in left, in right, out Matrix4X2D result);
            return result;
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        public static void Mult(in Matrix4X2D left, in Matrix2X3D right, out Matrix4X3D result)
        {
            double leftM11 = left.Row0.X;
            double leftM12 = left.Row0.Y;
            double leftM21 = left.Row1.X;
            double leftM22 = left.Row1.Y;
            double leftM31 = left.Row2.X;
            double leftM32 = left.Row2.Y;
            double leftM41 = left.Row3.X;
            double leftM42 = left.Row3.Y;
            double rightM11 = right.Row0.X;
            double rightM12 = right.Row0.Y;
            double rightM13 = right.Row0.Z;
            double rightM21 = right.Row1.X;
            double rightM22 = right.Row1.Y;
            double rightM23 = right.Row1.Z;

            result.Row0.X = (leftM11 * rightM11) + (leftM12 * rightM21);
            result.Row0.Y = (leftM11 * rightM12) + (leftM12 * rightM22);
            result.Row0.Z = (leftM11 * rightM13) + (leftM12 * rightM23);
            result.Row1.X = (leftM21 * rightM11) + (leftM22 * rightM21);
            result.Row1.Y = (leftM21 * rightM12) + (leftM22 * rightM22);
            result.Row1.Z = (leftM21 * rightM13) + (leftM22 * rightM23);
            result.Row2.X = (leftM31 * rightM11) + (leftM32 * rightM21);
            result.Row2.Y = (leftM31 * rightM12) + (leftM32 * rightM22);
            result.Row2.Z = (leftM31 * rightM13) + (leftM32 * rightM23);
            result.Row3.X = (leftM41 * rightM11) + (leftM42 * rightM21);
            result.Row3.Y = (leftM41 * rightM12) + (leftM42 * rightM22);
            result.Row3.Z = (leftM41 * rightM13) + (leftM42 * rightM23);
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        [Pure]
        public static Matrix4X3D Mult(Matrix4X2D left, Matrix2X3D right)
        {
            Mult(in left, in right, out Matrix4X3D result);
            return result;
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        public static void Mult(in Matrix4X2D left, in Matrix2X4D right, out Matrix4D result)
        {
            double leftM11 = left.Row0.X;
            double leftM12 = left.Row0.Y;
            double leftM21 = left.Row1.X;
            double leftM22 = left.Row1.Y;
            double leftM31 = left.Row2.X;
            double leftM32 = left.Row2.Y;
            double leftM41 = left.Row3.X;
            double leftM42 = left.Row3.Y;
            double rightM11 = right.Row0.X;
            double rightM12 = right.Row0.Y;
            double rightM13 = right.Row0.Z;
            double rightM14 = right.Row0.W;
            double rightM21 = right.Row1.X;
            double rightM22 = right.Row1.Y;
            double rightM23 = right.Row1.Z;
            double rightM24 = right.Row1.W;

            result.Row0.X = (leftM11 * rightM11) + (leftM12 * rightM21);
            result.Row0.Y = (leftM11 * rightM12) + (leftM12 * rightM22);
            result.Row0.Z = (leftM11 * rightM13) + (leftM12 * rightM23);
            result.Row0.W = (leftM11 * rightM14) + (leftM12 * rightM24);
            result.Row1.X = (leftM21 * rightM11) + (leftM22 * rightM21);
            result.Row1.Y = (leftM21 * rightM12) + (leftM22 * rightM22);
            result.Row1.Z = (leftM21 * rightM13) + (leftM22 * rightM23);
            result.Row1.W = (leftM21 * rightM14) + (leftM22 * rightM24);
            result.Row2.X = (leftM31 * rightM11) + (leftM32 * rightM21);
            result.Row2.Y = (leftM31 * rightM12) + (leftM32 * rightM22);
            result.Row2.Z = (leftM31 * rightM13) + (leftM32 * rightM23);
            result.Row2.W = (leftM31 * rightM14) + (leftM32 * rightM24);
            result.Row3.X = (leftM41 * rightM11) + (leftM42 * rightM21);
            result.Row3.Y = (leftM41 * rightM12) + (leftM42 * rightM22);
            result.Row3.Z = (leftM41 * rightM13) + (leftM42 * rightM23);
            result.Row3.W = (leftM41 * rightM14) + (leftM42 * rightM24);
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        [Pure]
        public static Matrix4D Mult(Matrix4X2D left, Matrix2X4D right)
        {
            Mult(in left, in right, out Matrix4D result);
            return result;
        }

        /// <summary>
        /// Adds two instances.
        /// </summary>
        /// <param name="left">The left operand of the addition.</param>
        /// <param name="right">The right operand of the addition.</param>
        /// <param name="result">A new instance that is the result of the addition.</param>
        public static void Add(in Matrix4X2D left, in Matrix4X2D right, out Matrix4X2D result)
        {
            result.Row0.X = left.Row0.X + right.Row0.X;
            result.Row0.Y = left.Row0.Y + right.Row0.Y;
            result.Row1.X = left.Row1.X + right.Row1.X;
            result.Row1.Y = left.Row1.Y + right.Row1.Y;
            result.Row2.X = left.Row2.X + right.Row2.X;
            result.Row2.Y = left.Row2.Y + right.Row2.Y;
            result.Row3.X = left.Row3.X + right.Row3.X;
            result.Row3.Y = left.Row3.Y + right.Row3.Y;
        }

        /// <summary>
        /// Adds two instances.
        /// </summary>
        /// <param name="left">The left operand of the addition.</param>
        /// <param name="right">The right operand of the addition.</param>
        /// <returns>A new instance that is the result of the addition.</returns>
        [Pure]
        public static Matrix4X2D Add(Matrix4X2D left, Matrix4X2D right)
        {
            Add(in left, in right, out Matrix4X2D result);
            return result;
        }

        /// <summary>
        /// Subtracts two instances.
        /// </summary>
        /// <param name="left">The left operand of the subtraction.</param>
        /// <param name="right">The right operand of the subtraction.</param>
        /// <param name="result">A new instance that is the result of the subtraction.</param>
        public static void Subtract(in Matrix4X2D left, in Matrix4X2D right, out Matrix4X2D result)
        {
            result.Row0.X = left.Row0.X - right.Row0.X;
            result.Row0.Y = left.Row0.Y - right.Row0.Y;
            result.Row1.X = left.Row1.X - right.Row1.X;
            result.Row1.Y = left.Row1.Y - right.Row1.Y;
            result.Row2.X = left.Row2.X - right.Row2.X;
            result.Row2.Y = left.Row2.Y - right.Row2.Y;
            result.Row3.X = left.Row3.X - right.Row3.X;
            result.Row3.Y = left.Row3.Y - right.Row3.Y;
        }

        /// <summary>
        /// Subtracts two instances.
        /// </summary>
        /// <param name="left">The left operand of the subtraction.</param>
        /// <param name="right">The right operand of the subtraction.</param>
        /// <returns>A new instance that is the result of the subtraction.</returns>
        [Pure]
        public static Matrix4X2D Subtract(Matrix4X2D left, Matrix4X2D right)
        {
            Subtract(in left, in right, out Matrix4X2D result);
            return result;
        }

        /// <summary>
        /// Calculate the transpose of the given matrix.
        /// </summary>
        /// <param name="mat">The matrix to transpose.</param>
        /// <param name="result">The transpose of the given matrix.</param>
        public static void Transpose(in Matrix4X2D mat, out Matrix2X4D result)
        {
            result.Row0.X = mat.Row0.X;
            result.Row0.Y = mat.Row1.X;
            result.Row0.Z = mat.Row2.X;
            result.Row0.W = mat.Row3.X;
            result.Row1.X = mat.Row0.Y;
            result.Row1.Y = mat.Row1.Y;
            result.Row1.Z = mat.Row2.Y;
            result.Row1.W = mat.Row3.Y;
        }

        /// <summary>
        /// Calculate the transpose of the given matrix.
        /// </summary>
        /// <param name="mat">The matrix to transpose.</param>
        /// <returns>The transpose of the given matrix.</returns>
        [Pure]
        public static Matrix2X4D Transpose(Matrix4X2D mat)
        {
            Transpose(in mat, out Matrix2X4D result);
            return result;
        }

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix4x2d which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix4X2D operator *(double left, Matrix4X2D right) => Mult(right, left);

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix4x2d which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix4X2D operator *(Matrix4X2D left, double right) => Mult(left, right);

        /// <summary>
        /// Matrix multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2d which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix4X2D operator *(Matrix4X2D left, Matrix2D right) => Mult(left, right);

        /// <summary>
        /// Matrix multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix4x3d which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix4X3D operator *(Matrix4X2D left, Matrix2X3D right) => Mult(left, right);

        /// <summary>
        /// Matrix multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix4d which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix4D operator *(Matrix4X2D left, Matrix2X4D right) => Mult(left, right);

        /// <summary>
        /// Matrix addition.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix4x2d which holds the result of the addition.</returns>
        [Pure]
        public static Matrix4X2D operator +(Matrix4X2D left, Matrix4X2D right) => Add(left, right);

        /// <summary>
        /// Matrix subtraction.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix4x2d which holds the result of the subtraction.</returns>
        [Pure]
        public static Matrix4X2D operator -(Matrix4X2D left, Matrix4X2D right) => Subtract(left, right);

        /// <summary>
        /// Compares two instances for equality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        [Pure]
        public static bool operator ==(Matrix4X2D left, Matrix4X2D right) => left.Equals(right);

        /// <summary>
        /// Compares two instances for inequality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equal right; false otherwise.</returns>
        [Pure]
        public static bool operator !=(Matrix4X2D left, Matrix4X2D right) => !left.Equals(right);

        /// <summary>
        /// Returns a System.String that represents the current Matrix3d.
        /// </summary>
        /// <returns>The string representation of the matrix.</returns>
        public override readonly string ToString() => ToString(null, null);

        /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
        public readonly string ToString(string format) => ToString(format, null);

        /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
        public readonly string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);

        /// <inheritdoc/>
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            string row0 = Row0.ToString(format, formatProvider);
            string row1 = Row1.ToString(format, formatProvider);
            string row2 = Row2.ToString(format, formatProvider);
            string row3 = Row3.ToString(format, formatProvider);
            return $"{row0}\n{row1}\n{row2}\n{row3}";
        }

        /// <summary>
        /// Returns the hashcode for this instance.
        /// </summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
        public override readonly int GetHashCode() => HashCode.Combine(Row0, Row1, Row2, Row3);
#pragma warning restore S2328 // "GetHashCode" should not reference mutable fields

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        [Pure]
        public override readonly bool Equals(object? obj) => obj is Matrix4X2D matrix && Equals(matrix);

        /// <summary>
        /// Indicates whether the current matrix is equal to another matrix.
        /// </summary>
        /// <param name="other">An matrix to compare with this matrix.</param>
        /// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
        [Pure]
        public readonly bool Equals(Matrix4X2D other) => Row0 == other.Row0 &&
                Row1 == other.Row1 &&
                Row2 == other.Row2 &&
                Row3 == other.Row3;
    }
}
