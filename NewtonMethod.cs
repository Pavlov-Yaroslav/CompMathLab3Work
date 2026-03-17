using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompMathLab2
{
    public static class NewtonMethod
    {
        public static float[] Iteration(float[] x, float[] b, ref int order)
        {
            float[] y = new float[x.Length];

            if (order != 0)
            {
                for (int i = 0; i < x.GetLength(0) - 1; i++)
                {
                    if (order == 0) break;
                    y[i] = (b[i] - b[i + 1]) / (x[i] - x[i + 1]);
                    order--;
                }
            }
            return y;
        }
        public static float[] Run(float[] x, float[] b, int order)
        {
            float[] y = x;

            for (int i = 0; i < order; i++)
            {
                int k = 1;
                if (k > 100) break;

                y = Iteration(y, b, ref order);
            }
            return y;
        }
        public static float Next(float[] x)
        { 
            for (;;)
            { 
            
            }
        }
    }
}
