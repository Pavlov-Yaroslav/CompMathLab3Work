using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompMathLab2
{
    public static class ZeydelMethod
    {
        public static void Start(float[,] temp, float[] tempB, double epsilon)
        {
            int rows = temp.GetLength(0);
            int columns = temp.GetLength(1);

            int columnsB = tempB.GetLength(0);

            float[,] A = new float[rows, columns];
            float[] b = new float[columnsB];


            for (int i = 0; i < rows; i++)
            {
                int max = 0;

                for (int j = 0; j < columns - 1; j++)
                {
                    if (temp[i, j + 1] > temp[i, j]) max = j + 1;
                }
                for (int j = 0; j < columns; j++)
                {
                    A[max, j] = temp[i, j];
                    b[max] = tempB[i];
                }
            }

            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("МАТРИЦА ПОСЛЕ ПЕРЕСТАНОВКИ:");
            PrintAugmentedMatrix(A, b, "Преобразованная система");

            //Проверка на диаганальную матрицу
            for (int i = 0; i < rows; i++)
            {
                float tmp = 0;
                for (int j = 0; j < columns; j++)
                {
                    if (j != i)
                    {
                        tmp += A[i, j];
                    }
                }

                if (A[i, i] < tmp)
                {
                    Console.WriteLine("\nВНИМАНИЕ: Матрица не имеет диагонального преобладания!");
                    Console.WriteLine("Метод Зейделя может расходиться.");
                    break;
                }
            }


            float[] x = new float[rows];
            float[] previous_x = new float[rows];

            for (int i = 0; i < rows; i++)
            {
                x[i] = b[i] / A[i, i];
            }

            int iteration = 0;

            Console.WriteLine($"\nПриближение: {iteration++}");
            for (int i = 0; i < rows; i++) Console.WriteLine($"x{i + 1} = {x[i]}");

            while (true)
            {
                Array.Copy(x, previous_x, rows);

                for (int i = 0; i < rows; i++)
                {
                    float tmp = b[i];

                    for (int j = 0; j < columns; j++)
                    {
                        if (j != i)
                        {
                            tmp -= x[j] * A[i, j];
                        }
                    }
                    x[i] = tmp / A[i, i];
                }

                Console.WriteLine($"\nПриближение: {iteration++}");
                for (int i = 0; i < rows; i++) Console.WriteLine($"x{i + 1} = {x[i]}");

                bool convergence = true;
                for (int i = 0; i < rows; i++)
                {
                    float diff = Math.Abs(x[i] - previous_x[i]) / Math.Max(Math.Abs(x[i]), 1e-10f);
                    if (float.IsNaN(x[i]) || float.IsInfinity(x[i]))
                    {
                        Console.WriteLine("Метод расходится!");
                        return;
                    }
                    Console.WriteLine($"Относительное изменение x{i + 1}: {diff}");
                    if (diff > epsilon) convergence = false;
                }

                if (convergence)
                {
                    Console.WriteLine($"\nРешение достигнуто за {--iteration} итераций");
                    break;
                }

                if (iteration > 1000)
                {
                    Console.WriteLine("Достигнут лимит итераций");
                    break;
                }
            }

        }

        static void PrintAugmentedMatrix(float[,] A, float[] b, string title = "Расширенная матрица")
        {
            Console.WriteLine($"\n{title}:");
            int n = A.GetLength(0);

            for (int i = 0; i < n; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"{A[i, j],8:F5} ");
                }
                Console.Write($"| {b[i],8:F5} |");
                Console.WriteLine();
            }
        }
    }
}

