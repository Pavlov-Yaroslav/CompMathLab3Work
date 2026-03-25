using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompMathLab2
{
    class LeastSquaresMethod
    {
        private float[] x;
        private float[] y;
        private int degree;
        private int n;

        public LeastSquaresMethod(float[] x, float[] y, int degree)
        {
            this.x = x;
            this.y = y;
            this.degree = degree;
            this.n = x.Length;
        }

        private float[] CalculateCM()
        {
            float[] c = new float[2 * degree + 1];

            for (int m = 0; m <= 2 * degree; m++)
            {
                float sum = 0;
                for (int i = 0; i < n; i++)
                {
                    sum += (float)Math.Pow(x[i], m);
                }
                c[m] = sum;
            }

            return c;
        }

        private float[] CalculateDJ()
        {
            float[] d = new float[degree + 1];

            for (int j = 0; j <= degree; j++)
            {
                float sum = 0;
                for (int i = 0; i < n; i++)
                {
                    sum += y[i] * (float)Math.Pow(x[i], j);
                }
                d[j] = sum;
            }

            return d;
        }

        private float[,] BuildCoefficientMatrix(float[] c)
        {
            int size = degree + 1;
            float[,] A = new float[size, size];

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    A[row, col] = c[row + col];
                }
            }

            return A;
        }

        public float[] Run()
        {
            if (degree >= n) throw new ArgumentException("Степень полинома должна быть меньше количества точек");

            float[] c = CalculateCM();

            float[] d = CalculateDJ();

            float[,] A = BuildCoefficientMatrix(c);

            float[,] extendedMatrix = GaussianMethod.CreateExtendedMatrix(A, d);

            var gaussSolver = new GaussianMainElement(extendedMatrix);
            gaussSolver.Solve();

            var solutionField = typeof(GaussianMainElement).GetField("solution", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            float[] coefficients = (float[])solutionField.GetValue(gaussSolver);

            return coefficients;
        }

        public float Evaluate(float[] coefficients, float xValue)
        {
            float result = 0;
            for (int i = 0; i < coefficients.Length; i++)
            {
                result += coefficients[i] * (float)Math.Pow(xValue, i);
            }
            return result;
        }
    }
}
