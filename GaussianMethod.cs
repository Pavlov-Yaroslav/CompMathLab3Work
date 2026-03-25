using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompMathLab2
{
    class GaussianSimple
    {
        private float[,] matrix;
        private int n;
        private float[] solution;

        public GaussianSimple(float[,] matrix)
        {
            this.matrix = (float[,])matrix.Clone();
            n = matrix.GetLength(0);
            solution = new float[n];
        }

        public void Solve()
        {
            Console.WriteLine("Исходная матрица:");
            PrintMatrix();

            ForwardMove();

            Console.WriteLine("Диагональная матрица:");
            PrintMatrix();

            BackwardMove();

            PrintSolution();
        }

        private void ForwardMove()
        {
            for (int k = 0; k < n - 1; k++)
            {
                if (Math.Abs(matrix[k, k]) < 1e-10) throw new Exception("Матрица вырождена");

                for (int i = k + 1; i < n; i++)
                {
                    float factor = matrix[i, k] / matrix[k, k];
                    for (int j = k; j <= n; j++)
                    {
                        matrix[i, j] -= factor * matrix[k, j];
                    }
                }
            }
        }

        private void BackwardMove()
        {
            for (int i = n - 1; i >= 0; i--)
            {
                solution[i] = matrix[i, n];
                for (int j = i + 1; j < n; j++)
                {
                    solution[i] -= matrix[i, j] * solution[j];
                }
                solution[i] /= matrix[i, i];
            }
        }

        private void PrintMatrix()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    Console.Write($"{matrix[i, j],12:F6} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void PrintSolution()
        {
            Console.WriteLine("Найденные x:");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"x{i + 1} = {solution[i]:F6}");
            }
        }

        public float[] GetSolution()
        {
            return solution;
        }
    }

    class GaussianMainElement
    {
        private float[,] matrix;
        private int n;
        private float[] solution;

        public GaussianMainElement(float[,] matrix)
        {
            this.matrix = (float[,])matrix.Clone();
            n = matrix.GetLength(0);
            solution = new float[n];
        }

        public void Solve()
        {
            Console.WriteLine("Исходная матрица:");
            PrintMatrix();

            ForwardMove();

            Console.WriteLine("Диагональная матрица:");
            PrintMatrix();

            BackwardMove();

            PrintSolution();
        }

        private void ForwardMove()
        {
            for (int k = 0; k < n - 1; k++)
            {
                int maxRow = FindMainRow(k);
                if (maxRow != k)
                    SwapRows(k, maxRow);

                if (Math.Abs(matrix[k, k]) < 1e-10) throw new Exception("Матрица вырождена");

                SetZeroElements(k);
            }
        }

        private int FindMainRow(int k)
        {
            int maxRow = k;
            float maxValue = Math.Abs(matrix[k, k]);

            for (int i = k + 1; i < n; i++)
            {
                if (Math.Abs(matrix[i, k]) > maxValue)
                {
                    maxValue = Math.Abs(matrix[i, k]);
                    maxRow = i;
                }
            }
            return maxRow;
        }

        private void SwapRows(int row1, int row2)
        {
            for (int j = 0; j <= n; j++)
            {
                float temp = matrix[row1, j];
                matrix[row1, j] = matrix[row2, j];
                matrix[row2, j] = temp;
            }
        }

        private void SetZeroElements(int k)
        {
            for (int i = k + 1; i < n; i++)
            {
                float factor = matrix[i, k] / matrix[k, k];
                for (int j = k; j <= n; j++)
                {
                    matrix[i, j] -= factor * matrix[k, j];
                }
            }
        }

        private void BackwardMove()
        {
            for (int i = n - 1; i >= 0; i--)
            {
                solution[i] = matrix[i, n];
                for (int j = i + 1; j < n; j++)
                {
                    solution[i] -= matrix[i, j] * solution[j];
                }
                solution[i] /= matrix[i, i];
            }
        }

        private void PrintMatrix()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    Console.Write($"{matrix[i, j],12:F6} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void PrintSolution()
        {
            Console.WriteLine("Найденные x:");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"x{i + 1} = {solution[i]:F6}");
            }
        }

        public float[] GetSolution()
        {
            return solution;
        }
    }

    public static class GaussianMethod
    {
        public static float[,] CreateExtendedMatrix(float[,] A, float[] b)
        {
            int rows = A.GetLength(0);
            int cols = A.GetLength(1);

            if (b.Length != rows)
                throw new ArgumentException("Размер вектора b не совпадает с количеством строк матрицы A");

            float[,] result = new float[rows, cols + 1];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = A[i, j];
                }
                result[i, cols] = b[i];
            }

            return result;
        }

        public static float[] SolveInterpolationSystem(double[] x, double[] y)
        {
            int n = x.Length;

            float[,] A = new float[n, n];
            float[] b = new float[n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = (float)Math.Pow(x[i], j);
                }
                b[i] = (float)y[i];
            }

            float[,] extendedMatrix = CreateExtendedMatrix(A, b);

            var gaussSolver = new GaussianMainElement(extendedMatrix);
            gaussSolver.Solve();

            return gaussSolver.GetSolution();
        }

        public static void Start(float[,] A, float[] b)
        {
            float[,] Matrix = CreateExtendedMatrix(A, b);

            Console.WriteLine("\n1. Метод Гаусса без выбора главного элемента:");
            try
            {
                var matr1 = new GaussianSimple(Matrix);
                matr1.Solve();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine("\n2. Метод Гаусса с выбором главного элемента:");
            var matr2 = new GaussianMainElement(Matrix);
            matr2.Solve();
        }
    }
}
