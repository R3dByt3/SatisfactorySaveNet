using SatisfactorySaveNet.Abstracts.Maths.Vector;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace SatisfactorySaveNet.Abstracts.Maths.Matrix
{
    /// <summary>
    /// Represents a 2x4 matrix.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix2X4D : IEquatable<Matrix2X4D>, IFormattable
    {
        /// <summary>
        /// Top row of the matrix.
        /// </summary>
        public Vector4D Row0;

        /// <summary>
        /// Bottom row of the matrix.
        /// </summary>
        public Vector4D Row1;

        /// <summary>
        /// The zero matrix.
        /// </summary>
        public static readonly Matrix2X4D Zero = new(Vector4D.Zero, Vector4D.Zero);

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix2X4D"/> struct.
        /// </summary>
        /// <param name="row0">Top row of the matrix.</param>
        /// <param name="row1">Bottom row of the matrix.</param>
        public Matrix2X4D(Vector4D row0, Vector4D row1)
        {
            Row0 = row0;
            Row1 = row1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix2X4D"/> struct.
        /// </summary>
        /// <param name="m00">First item of the first row of the matrix.</param>
        /// <param name="m01">Second item of the first row of the matrix.</param>
        /// <param name="m02">Third item of the first row of the matrix.</param>
        /// <param name="m03">Fourth item of the first row of the matrix.</param>
        /// <param name="m10">First item of the second row of the matrix.</param>
        /// <param name="m11">Second item of the second row of the matrix.</param>
        /// <param name="m12">Third item of the second row of the matrix.</param>
        /// <param name="m13">Fourth item of the second row of the matrix.</param>
        public Matrix2X4D
        (
            double m00, double m01, double m02, double m03,
            double m10, double m11, double m12, double m13
        )
        {
            Row0 = new Vector4D(m00, m01, m02, m03);
            Row1 = new Vector4D(m10, m11, m12, m13);
        }

        /// <summary>
        /// Gets or sets the first column of the matrix.
        /// </summary>
        public Vector2D Column0
        {
            readonly get => new(Row0.X, Row1.X);
            set
            {
                Row0.X = value.X;
                Row1.X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the second column of the matrix.
        /// </summary>
        public Vector2D Column1
        {
            readonly get => new(Row0.Y, Row1.Y);
            set
            {
                Row0.Y = value.X;
                Row1.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the third column of the matrix.
        /// </summary>
        public Vector2D Column2
        {
            readonly get => new(Row0.Z, Row1.Z);
            set
            {
                Row0.Z = value.X;
                Row1.Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the fourth column of the matrix.
        /// </summary>
        public Vector2D Column3
        {
            readonly get => new(Row0.W, Row1.W);
            set
            {
                Row0.W = value.X;
                Row1.W = value.Y;
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
        /// Gets or sets the value at row 1, column 3 of this instance.
        /// </summary>
        public double M13
        {
            readonly get => Row0.Z;
            set => Row0.Z = value;
        }

        /// <summary>
        /// Gets or sets the value at row 1, column 4 of this instance.
        /// </summary>
        public double M14
        {
            readonly get => Row0.W;
            set => Row0.W = value;
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
        /// Gets or sets the value at row 2, column 3 of this instance.
        /// </summary>
        public double M23
        {
            readonly get => Row1.Z;
            set => Row1.Z = value;
        }

        /// <summary>
        /// Gets or sets the value at row 2, column 4 of this instance.
        /// </summary>
        public double M24
        { 
            readonly get => Row1.W;
            set => Row1.W = value;
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
                var tmp = rowIndex == 0
                    ? Row0[columnIndex]
                    : rowIndex;
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                return tmp == 1
                    ? Row1[columnIndex]
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
                else
                {
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                    Row1[columnIndex] = rowIndex == 1
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
        /// <param name="result">The resulting Matrix2x4d instance.</param>
        public static void CreateRotation(double angle, out Matrix2X4D result)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            result.Row0.X = cos;
            result.Row0.Y = sin;
            result.Row0.Z = 0;
            result.Row0.W = 0;
            result.Row1.X = -sin;
            result.Row1.Y = cos;
            result.Row1.Z = 0;
            result.Row1.W = 0;
        }

        /// <summary>
        /// Builds a rotation matrix.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix2x3d instance.</returns>
        [Pure]
        public static Matrix2X4D CreateRotation(double angle)
        {
            CreateRotation(angle, out Matrix2X4D result);
            return result;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="scale">Single scale factor for the x, y, and z axes.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(double scale, out Matrix2X4D result)
        {
            result.Row0.X = scale;
            result.Row0.Y = 0;
            result.Row0.Z = 0;
            result.Row0.W = 0;
            result.Row1.X = 0;
            result.Row1.Y = scale;
            result.Row1.Z = 0;
            result.Row1.W = 0;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="scale">Single scale factor for the x and y axes.</param>
        /// <returns>A scale matrix.</returns>
        [Pure]
        public static Matrix2X4D CreateScale(double scale)
        {
            CreateScale(scale, out Matrix2X4D result);
            return result;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="scale">Scale factors for the x and y axes.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(Vector2D scale, out Matrix2X4D result)
        {
            result.Row0.X = scale.X;
            result.Row0.Y = 0;
            result.Row0.Z = 0;
            result.Row0.W = 0;
            result.Row1.X = 0;
            result.Row1.Y = scale.Y;
            result.Row1.Z = 0;
            result.Row1.W = 0;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="scale">Scale factors for the x and y axes.</param>
        /// <returns>A scale matrix.</returns>
        [Pure]
        public static Matrix2X4D CreateScale(Vector2D scale)
        {
            CreateScale(scale, out Matrix2X4D result);
            return result;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="x">Scale factor for the x axis.</param>
        /// <param name="y">Scale factor for the y axis.</param>
        /// <param name="result">A scale matrix.</param>
        public static void CreateScale(double x, double y, out Matrix2X4D result)
        {
            result.Row0.X = x;
            result.Row0.Y = 0;
            result.Row0.Z = 0;
            result.Row0.W = 0;
            result.Row1.X = 0;
            result.Row1.Y = y;
            result.Row1.Z = 0;
            result.Row1.W = 0;
        }

        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        /// <param name="x">Scale factor for the x axis.</param>
        /// <param name="y">Scale factor for the y axis.</param>
        /// <returns>A scale matrix.</returns>
        [Pure]
        public static Matrix2X4D CreateScale(double x, double y)
        {
            CreateScale(x, y, out Matrix2X4D result);
            return result;
        }

        /// <summary>
        /// Multiplies and instance by a scalar.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        public static void Mult(in Matrix2X4D left, double right, out Matrix2X4D result)
        {
            result.Row0.X = left.Row0.X * right;
            result.Row0.Y = left.Row0.Y * right;
            result.Row0.Z = left.Row0.Z * right;
            result.Row0.W = left.Row0.W * right;
            result.Row1.X = left.Row1.X * right;
            result.Row1.Y = left.Row1.Y * right;
            result.Row1.Z = left.Row1.Z * right;
            result.Row1.W = left.Row1.W * right;
        }

        /// <summary>
        /// Multiplies and instance by a scalar.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        [Pure]
        public static Matrix2X4D Mult(Matrix2X4D left, double right)
        {
            Mult(in left, right, out Matrix2X4D result);
            return result;
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        public static void Mult(in Matrix2X4D left, in Matrix4X2 right, out Matrix2D result)
        {
            double leftM11 = left.Row0.X;
            double leftM12 = left.Row0.Y;
            double leftM13 = left.Row0.Z;
            double leftM14 = left.Row0.W;
            double leftM21 = left.Row1.X;
            double leftM22 = left.Row1.Y;
            double leftM23 = left.Row1.Z;
            double leftM24 = left.Row1.W;
            double rightM11 = right.Row0.X;
            double rightM12 = right.Row0.Y;
            double rightM21 = right.Row1.X;
            double rightM22 = right.Row1.Y;
            double rightM31 = right.Row2.X;
            double rightM32 = right.Row2.Y;
            double rightM41 = right.Row3.X;
            double rightM42 = right.Row3.Y;

            result.Row0.X = (leftM11 * rightM11) + (leftM12 * rightM21) + (leftM13 * rightM31) + (leftM14 * rightM41);
            result.Row0.Y = (leftM11 * rightM12) + (leftM12 * rightM22) + (leftM13 * rightM32) + (leftM14 * rightM42);
            result.Row1.X = (leftM21 * rightM11) + (leftM22 * rightM21) + (leftM23 * rightM31) + (leftM24 * rightM41);
            result.Row1.Y = (leftM21 * rightM12) + (leftM22 * rightM22) + (leftM23 * rightM32) + (leftM24 * rightM42);
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        [Pure]
        public static Matrix2D Mult(Matrix2X4D left, Matrix4X2 right)
        {
            Mult(in left, in right, out Matrix2D result);
            return result;
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        public static void Mult(in Matrix2X4D left, in Matrix4X3 right, out Matrix2X3D result)
        {
            double leftM11 = left.Row0.X;
            double leftM12 = left.Row0.Y;
            double leftM13 = left.Row0.Z;
            double leftM14 = left.Row0.W;
            double leftM21 = left.Row1.X;
            double leftM22 = left.Row1.Y;
            double leftM23 = left.Row1.Z;
            double leftM24 = left.Row1.W;
            double rightM11 = right.Row0.X;
            double rightM12 = right.Row0.Y;
            double rightM13 = right.Row0.Z;
            double rightM21 = right.Row1.X;
            double rightM22 = right.Row1.Y;
            double rightM23 = right.Row1.Z;
            double rightM31 = right.Row2.X;
            double rightM32 = right.Row2.Y;
            double rightM33 = right.Row2.Z;
            double rightM41 = right.Row3.X;
            double rightM42 = right.Row3.Y;
            double rightM43 = right.Row3.Z;

            result.Row0.X = (leftM11 * rightM11) + (leftM12 * rightM21) + (leftM13 * rightM31) + (leftM14 * rightM41);
            result.Row0.Y = (leftM11 * rightM12) + (leftM12 * rightM22) + (leftM13 * rightM32) + (leftM14 * rightM42);
            result.Row0.Z = (leftM11 * rightM13) + (leftM12 * rightM23) + (leftM13 * rightM33) + (leftM14 * rightM43);
            result.Row1.X = (leftM21 * rightM11) + (leftM22 * rightM21) + (leftM23 * rightM31) + (leftM24 * rightM41);
            result.Row1.Y = (leftM21 * rightM12) + (leftM22 * rightM22) + (leftM23 * rightM32) + (leftM24 * rightM42);
            result.Row1.Z = (leftM21 * rightM13) + (leftM22 * rightM23) + (leftM23 * rightM33) + (leftM24 * rightM43);
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        [Pure]
        public static Matrix2X3D Mult(Matrix2X4D left, Matrix4X3 right)
        {
            Mult(in left, in right, out Matrix2X3D result);
            return result;
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        public static void Mult(in Matrix2X4D left, in Matrix4 right, out Matrix2X4D result)
        {
            double leftM11 = left.Row0.X;
            double leftM12 = left.Row0.Y;
            double leftM13 = left.Row0.Z;
            double leftM14 = left.Row0.W;
            double leftM21 = left.Row1.X;
            double leftM22 = left.Row1.Y;
            double leftM23 = left.Row1.Z;
            double leftM24 = left.Row1.W;
            double rightM11 = right.Row0.X;
            double rightM12 = right.Row0.Y;
            double rightM13 = right.Row0.Z;
            double rightM14 = right.Row0.W;
            double rightM21 = right.Row1.X;
            double rightM22 = right.Row1.Y;
            double rightM23 = right.Row1.Z;
            double rightM24 = right.Row1.W;
            double rightM31 = right.Row2.X;
            double rightM32 = right.Row2.Y;
            double rightM33 = right.Row2.Z;
            double rightM34 = right.Row2.W;
            double rightM41 = right.Row3.X;
            double rightM42 = right.Row3.Y;
            double rightM43 = right.Row3.Z;
            double rightM44 = right.Row3.W;

            result.Row0.X = (leftM11 * rightM11) + (leftM12 * rightM21) + (leftM13 * rightM31) + (leftM14 * rightM41);
            result.Row0.Y = (leftM11 * rightM12) + (leftM12 * rightM22) + (leftM13 * rightM32) + (leftM14 * rightM42);
            result.Row0.Z = (leftM11 * rightM13) + (leftM12 * rightM23) + (leftM13 * rightM33) + (leftM14 * rightM43);
            result.Row0.W = (leftM11 * rightM14) + (leftM12 * rightM24) + (leftM13 * rightM34) + (leftM14 * rightM44);
            result.Row1.X = (leftM21 * rightM11) + (leftM22 * rightM21) + (leftM23 * rightM31) + (leftM24 * rightM41);
            result.Row1.Y = (leftM21 * rightM12) + (leftM22 * rightM22) + (leftM23 * rightM32) + (leftM24 * rightM42);
            result.Row1.Z = (leftM21 * rightM13) + (leftM22 * rightM23) + (leftM23 * rightM33) + (leftM24 * rightM43);
            result.Row1.W = (leftM21 * rightM14) + (leftM22 * rightM24) + (leftM23 * rightM34) + (leftM24 * rightM44);
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        [Pure]
        public static Matrix2X4D Mult(Matrix2X4D left, Matrix4 right)
        {
            Mult(in left, in right, out Matrix2X4D result);
            return result;
        }

        /// <summary>
        /// Adds two instances.
        /// </summary>
        /// <param name="left">The left operand of the addition.</param>
        /// <param name="right">The right operand of the addition.</param>
        /// <param name="result">A new instance that is the result of the addition.</param>
        public static void Add(in Matrix2X4D left, in Matrix2X4D right, out Matrix2X4D result)
        {
            result.Row0.X = left.Row0.X + right.Row0.X;
            result.Row0.Y = left.Row0.Y + right.Row0.Y;
            result.Row0.Z = left.Row0.Z + right.Row0.Z;
            result.Row0.W = left.Row0.W + right.Row0.W;
            result.Row1.X = left.Row1.X + right.Row1.X;
            result.Row1.Y = left.Row1.Y + right.Row1.Y;
            result.Row1.Z = left.Row1.Z + right.Row1.Z;
            result.Row1.W = left.Row1.W + right.Row1.W;
        }

        /// <summary>
        /// Adds two instances.
        /// </summary>
        /// <param name="left">The left operand of the addition.</param>
        /// <param name="right">The right operand of the addition.</param>
        /// <returns>A new instance that is the result of the addition.</returns>
        [Pure]
        public static Matrix2X4D Add(Matrix2X4D left, Matrix2X4D right)
        {
            Add(in left, in right, out Matrix2X4D result);
            return result;
        }

        /// <summary>
        /// Subtracts two instances.
        /// </summary>
        /// <param name="left">The left operand of the subtraction.</param>
        /// <param name="right">The right operand of the subtraction.</param>
        /// <param name="result">A new instance that is the result of the subtraction.</param>
        public static void Subtract(in Matrix2X4D left, in Matrix2X4D right, out Matrix2X4D result)
        {
            result.Row0.X = left.Row0.X - right.Row0.X;
            result.Row0.Y = left.Row0.Y - right.Row0.Y;
            result.Row0.Z = left.Row0.Z - right.Row0.Z;
            result.Row0.W = left.Row0.W - right.Row0.W;
            result.Row1.X = left.Row1.X - right.Row1.X;
            result.Row1.Y = left.Row1.Y - right.Row1.Y;
            result.Row1.Z = left.Row1.Z - right.Row1.Z;
            result.Row1.W = left.Row1.W - right.Row1.W;
        }

        /// <summary>
        /// Subtracts two instances.
        /// </summary>
        /// <param name="left">The left operand of the subtraction.</param>
        /// <param name="right">The right operand of the subtraction.</param>
        /// <returns>A new instance that is the result of the subtraction.</returns>
        [Pure]
        public static Matrix2X4D Subtract(Matrix2X4D left, Matrix2X4D right)
        {
            Subtract(in left, in right, out Matrix2X4D result);
            return result;
        }

        /// <summary>
        /// Calculate the transpose of the given matrix.
        /// </summary>
        /// <param name="mat">The matrix to transpose.</param>
        /// <param name="result">The transpose of the given matrix.</param>
        public static void Transpose(in Matrix2X4D mat, out Matrix4X2D result)
        {
            result.Row0.X = mat.Row0.X;
            result.Row0.Y = mat.Row1.X;
            result.Row1.X = mat.Row0.Y;
            result.Row1.Y = mat.Row1.Y;
            result.Row2.X = mat.Row0.Z;
            result.Row2.Y = mat.Row1.Z;
            result.Row3.X = mat.Row0.W;
            result.Row3.Y = mat.Row1.W;
        }

        /// <summary>
        /// Calculate the transpose of the given matrix.
        /// </summary>
        /// <param name="mat">The matrix to transpose.</param>
        /// <returns>The transpose of the given matrix.</returns>
        [Pure]
        public static Matrix4X2D Transpose(Matrix2X4D mat)
        {
            Transpose(in mat, out Matrix4X2D result);
            return result;
        }

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2x4d which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix2X4D operator *(double left, Matrix2X4D right) => Mult(right, left);

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2x4d which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix2X4D operator *(Matrix2X4D left, double right) => Mult(left, right);

        /// <summary>
        /// Matrix multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2d which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix2D operator *(Matrix2X4D left, Matrix4X2 right) => Mult(left, right);

        /// <summary>
        /// Matrix multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2x3d which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix2X3D operator *(Matrix2X4D left, Matrix4X3 right) => Mult(left, right);

        /// <summary>
        /// Matrix multiplication.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2x4d which holds the result of the multiplication.</returns>
        [Pure]
        public static Matrix2X4D operator *(Matrix2X4D left, Matrix4 right) => Mult(left, right);

        /// <summary>
        /// Matrix addition.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2d which holds the result of the addition.</returns>
        [Pure]
        public static Matrix2X4D operator +(Matrix2X4D left, Matrix2X4D right) => Add(left, right);

        /// <summary>
        /// Matrix subtraction.
        /// </summary>
        /// <param name="left">left-hand operand.</param>
        /// <param name="right">right-hand operand.</param>
        /// <returns>A new Matrix2x4d which holds the result of the subtraction.</returns>
        [Pure]
        public static Matrix2X4D operator -(Matrix2X4D left, Matrix2X4D right) => Subtract(left, right);

        /// <summary>
        /// Compares two instances for equality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        [Pure]
        public static bool operator ==(Matrix2X4D left, Matrix2X4D right) => left.Equals(right);

        /// <summary>
        /// Compares two instances for inequality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equal right; false otherwise.</returns>
        [Pure]
        public static bool operator !=(Matrix2X4D left, Matrix2X4D right) => !left.Equals(right);

        /// <summary>
        /// Returns a System.String that represents the current Matrix4.
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
            return $"{row0}\n{row1}";
        }

        /// <summary>
        /// Returns the hashcode for this instance.
        /// </summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
        public override readonly int GetHashCode() => HashCode.Combine(Row0, Row1);
#pragma warning restore S2328 // "GetHashCode" should not reference mutable fields

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        [Pure]
        public override readonly bool Equals(object? obj) => obj is Matrix2X4D matrix && Equals(matrix);

        /// <summary>
        /// Indicates whether the current matrix is equal to another matrix.
        /// </summary>
        /// <param name="other">An matrix to compare with this matrix.</param>
        /// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
        [Pure]
        public readonly bool Equals(Matrix2X4D other) => Row0 == other.Row0 &&
                Row1 == other.Row1;
    }
}
