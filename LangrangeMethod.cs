using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CompMathLab3
{
    public static class LagrangeMethod
    {
        public static float Run(float[] x, float[] y, float unknown)
        {
            if (x.Length != y.Length) throw new ArgumentException("Массивы x и y должны быть одинаковой длины");

            float result = 0;
            int n = x.Length;

            for (int i = 0; i < n; i++)
            {
                float term = y[i];

                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        term *= (unknown - x[j]) / (x[i] - x[j]);
                    }
                }

                result += term;
            }

            return result;
        }

        public static float[] Run(float[] x, float[] y, float[] unknownPoints)
        {
            float[] results = new float[unknownPoints.Length];

            for (int k = 0; k < unknownPoints.Length; k++)
            {
                results[k] = Run(x, y, unknownPoints[k]);
            }

            return results;
        }
    }
}

