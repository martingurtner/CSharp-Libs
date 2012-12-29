using System;
using System.Text.RegularExpressions;

namespace can_gui
{
    public class Matrix
    {
        public int rows;
        public int cols;
        public double[,] mat;

        /// <summary>
        /// Matrix Class constructor.
        /// </summary>
        /// <param name="rows"> Number of rows in the created matrix.</param>
        /// <param name="cols"> Number of columns in the created matrix.</param>
        public Matrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            mat = new double[rows, cols];
        }

        /// <summary>
        /// Access a component in this matrix as a double array.
        /// </summary>
        /// <param name="i"> Row index of the component.</param>
        /// <param name="j"> Column index of the component.</param>
        public double this[int i, int j]
        {
            get { return mat[i, j]; }
            set { mat[i, j] = value; }
        }

        /// <summary>
        /// Duplicate this matrix.
        /// </summary>
        /// <returns> Copy of this matrix. </returns>
        public Matrix Copy()
        {
            Matrix m = new Matrix(this.rows, this.cols);
            for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    m[i, j] = mat[i, j];
            return m;
        }

        /// <summary>
        /// Generate matrix full of zeros.
        /// </summary>
        /// <param name="rows"> Number of rows in the created matrix.</param>
        /// <param name="cols"> Number of columns in the created matrix.</param>
        /// <returns> Retruns matrix full of zeros. </returns>
        public static Matrix ZeroMatrix(int rows, int cols)
        {
            Matrix m = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    m[i, j] = 0;
            return m;
        }

        /// <summary>
        /// Generate an identity matrix.
        /// </summary>
        /// <param name="n"> Number of rows and columns in the created matrix.</param>
        /// <returns> Retruns a identity matrix. </returns>
        public static Matrix IdentMatrix(int n)
        {
            Matrix m = ZeroMatrix(n, n);
            for (int i = 0; i < n; i++)
                m[i, i] = 1;
            return m;
        }

        /// <summary>
        /// Convert this matrix to text.
        /// </summary>
        /// <returns> Returns this matrix as a text.. </returns>
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++) s += String.Format("{0,6:0.000}", mat[i, j]) + " ";
                s += "\r\n";
            }
            return s;
        }

        /// <summary>
        /// Transposition of a matrix.
        /// </summary>
        /// <param name="m"> The matrix to transpose. </param>
        /// <returns> Transposited matrix <paramref name="m"/>. </returns>
        public static Matrix Transpose(Matrix m)
        {
            Matrix t = new Matrix(m.cols, m.rows);
            for (int i = 0; i < m.rows; i++)
                for (int j = 0; j < m.cols; j++)
                    t[j, i] = m[i, j];
            return t;
        }

        /// <summary>
        /// Multiplication of matrices.
        /// </summary>
        /// <param name="m1"> The multiplier. </param>
        /// <param name="m2"> The multiplicand. </param>
        /// <returns> <paramref name="m1"/>*<paramref name="m2"/>. </returns>
        public static Matrix Multiply(Matrix m1, Matrix m2)
        {
            if (m1.cols != m2.rows) throw new MatrixException("Matrixes cannot be multiplied because of their dimensions!");

            Matrix result = ZeroMatrix(m1.rows, m2.cols);
            for (int i = 0; i < result.rows; i++)
                for (int j = 0; j < result.cols; j++)
                    for (int k = 0; k < m1.cols; k++)
                        result[i, j] += m1[i, k] * m2[k, j];
            return result;
        }

        /// <summary>
        /// Scalar multiplication of a matrix.
        /// </summary>
        /// <param name="n"> The scalar multiplier. </param>
        /// <param name="m"> The matrix multiplicand. </param>
        /// <returns> <paramref name="n"/>*<paramref name="m"/>. </returns>
        private static Matrix ScalarMultiply(double n, Matrix m)
        {
            Matrix r = new Matrix(m.rows, m.cols);
            for (int i = 0; i < m.rows; i++)
                for (int j = 0; j < m.cols; j++)
                    r[i, j] = m[i, j] * n;
            return r;
        }

        /// <summary>
        /// Addition of a matrices.
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns> <paramref name="m1"/>+<paramref name="m2"/>. </returns>
        private static Matrix Add(Matrix m1, Matrix m2)
        {
            if (m1.rows != m2.rows || m1.cols != m2.cols) throw new MatrixException("Matrices must have the same dimensions!");
            Matrix r = new Matrix(m1.rows, m1.cols);
            for (int i = 0; i < r.rows; i++)
                for (int j = 0; j < r.cols; j++)
                    r[i, j] = m1[i, j] + m2[i, j];
            return r;
        }

        /// <summary>
        /// Generate a four dimensional rotation matrix according to euler angles in ZYX convention.
        /// </summary>
        /// <param name="yaw"> Yaw angle</param>
        /// <param name="pitch"> Pitch angle</param>
        /// <param name="roll"> Roll angle</param>
        /// <returns> Rotation matrix.</returns>
        public static Matrix EulerRotaionMatrix(double yaw, double pitch, double roll)
        {
            return RotaionMatrixX4(roll) * RotaionMatrixY4(pitch) * RotaionMatrixZ4(yaw);
        }

        /// <summary>
        /// Generate a four dimensional rotation matrix which rotates a vector around the Z axis.
        /// </summary>
        /// <param name="angle">angle</param>
        /// <returns> Rotation matrix.</returns>
        public static Matrix RotaionMatrixZ4(double angle)
        {
            Matrix m = IdentMatrix(4);
            m[0, 0] = Math.Cos(angle);
            m[0, 1] = -Math.Sin(angle);

            m[1, 0] = Math.Sin(angle);
            m[1, 1] = Math.Cos(angle);

            return m;
        }

        /// <summary>
        /// Generate a four dimensional rotation matrix which rotates a vector around the X axis.
        /// </summary>
        /// <param name="angle">angle</param>
        /// <returns> Rotation matrix.</returns>
        public static Matrix RotaionMatrixX4(double angle)
        {
            Matrix m = IdentMatrix(4);
            m[1, 1] = Math.Cos(angle);
            m[1, 2] = -Math.Sin(angle);

            m[2, 1] = Math.Sin(angle);
            m[2, 2] = Math.Cos(angle);

            return m;
        }

        /// <summary>
        /// Generate a four dimensional rotation matrix which rotates a vector around the Y axis.
        /// </summary>
        /// <param name="angle">angle</param>
        /// <returns> Rotation matrix.</returns>
        public static Matrix RotaionMatrixY4(double angle)
        {            
            Matrix m = IdentMatrix(4);

            m[0, 0] = Math.Cos(angle);
            m[0, 2] = Math.Sin(angle);

            m[2, 0] = -Math.Sin(angle);
            m[2, 2] = Math.Cos(angle);

            return m;
        }

        /// <summary>
        /// Generate a four dimensional translation matrix.
        /// </summary>
        /// <param name="xTrans"> Translation along the X axis. </param>
        /// <param name="yTrans"> Translation along the Y axis. </param>
        /// <param name="zTrans"> Translation along the Z axis. </param>
        /// <returns> Translation matrix.</returns>
        public static Matrix TranslationMatrix(double xTrans, double yTrans, double zTrans)
        {
            Matrix m = IdentMatrix(4);

            m[0, 3] = xTrans;
            m[1, 3] = yTrans;
            m[2, 3] = zTrans;

            return m;
        }

        /// <summary>
        /// Generate a homogenous transformation matrix according to given Denavit-Hartenberg parameters.
        /// </summary>
        /// <param name="DHparams"> An array of Denavit-Hartenberg parameters in usual order (theta, d, a, alpha). </param>
        /// <returns> Translation matrix.</returns>
        public static Matrix DHTransMatrix(double[][] DHparams)
        {
            Matrix T = IdentMatrix(4);

            for (int i = 0; i < DHparams.Length; i++)
            {
                Matrix jointT = IdentMatrix(4);
                jointT[0, 0] = Math.Cos(DHparams[i][0]);
                jointT[0, 1] = -Math.Sin(DHparams[i][0])*Math.Cos(DHparams[i][3]);
                jointT[0, 2] = Math.Sin(DHparams[i][0])*Math.Sin(DHparams[i][3]);
                jointT[0, 3] = DHparams[i][2]*Math.Cos(DHparams[i][0]);

                jointT[1, 0] = Math.Sin(DHparams[i][0]);
                jointT[1, 1] = Math.Cos(DHparams[i][0])*Math.Cos(DHparams[i][3]);
                jointT[1, 2] = -Math.Cos(DHparams[i][0])*Math.Sin(DHparams[i][3]);
                jointT[1, 3] = DHparams[i][2]*Math.Sin(DHparams[i][0]);

                jointT[2, 0] = 0;
                jointT[2, 1] = Math.Sin(DHparams[i][3]);
                jointT[2, 2] = Math.Cos(DHparams[i][3]);
                jointT[2, 3] = DHparams[i][1];

                T = T * jointT;
            }           

            return T;
        }

        //   O P E R A T O R S

        public static Matrix operator -(Matrix m)
        { return Matrix.ScalarMultiply(-1, m); }

        public static Matrix operator +(Matrix m1, Matrix m2)
        { return Matrix.Add(m1, m2); }

        public static Matrix operator -(Matrix m1, Matrix m2)
        { return Matrix.Add(m1, -m2); }

        public static Matrix operator *(Matrix m1, Matrix m2)
        { return Matrix.Multiply(m1, m2); }

        public static Matrix operator *(double n, Matrix m)
        { return Matrix.ScalarMultiply(n, m); }
    }

    public class MatrixException : Exception
    {
        public MatrixException(string Message):base(Message) { }
    }

}