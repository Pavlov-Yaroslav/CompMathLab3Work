using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CompMathLab3
{
    public partial class Form1 : Form
    {
        private double[] xValues = { 8, 10, 12, 14, 16 };
        private double[] yValues = { 4, 9, 5, 1, 16 };

        // Коэффициенты многочлена Гаусса (из решения системы)
        private double a0 = 64;
        private double a1 = -54.6666666666667;
        private double a2 = 11.8958333333333;
        private double a3 = -0.958333333333333;
        private double a4 = 0.0260416666666667;

        public Form1()
        {
            InitializeComponent();
            DrawAllGraphs();
        }

        private void DrawAllGraphs()
        {
            DrawChart1(); // Гаусс на chart1
            DrawChart2(); // Лагранж на chart2
            DrawChart3(); // Сравнение на chart3
        }

        private void DrawChart1()
        {
            chart1.Series.Clear();
            
            // Настройка области
            if (chart1.ChartAreas.Count == 0)
                chart1.ChartAreas.Add(new ChartArea());
            
            chart1.ChartAreas[0].AxisX.Title = "x";
            chart1.ChartAreas[0].AxisY.Title = "y";
            chart1.ChartAreas[0].AxisX.Minimum = 7;
            chart1.ChartAreas[0].AxisX.Maximum = 17;
            chart1.ChartAreas[0].AxisY.Minimum = -5;
            chart1.ChartAreas[0].AxisY.Maximum = 20;

            // Серия для графика Гаусса
            Series gaussSeries = new Series("Метод Гаусса");
            gaussSeries.ChartType = SeriesChartType.Line;
            gaussSeries.Color = Color.Blue;
            gaussSeries.BorderWidth = 2;

            // Серия для точек
            Series pointSeries = new Series("Исходные точки");
            pointSeries.ChartType = SeriesChartType.Point;
            pointSeries.Color = Color.Red;
            pointSeries.MarkerSize = 8;
            pointSeries.MarkerStyle = MarkerStyle.Circle;

            // Добавляем точки
            for (int i = 0; i < xValues.Length; i++)
            {
                pointSeries.Points.AddXY(xValues[i], yValues[i]);
            }

            // Строим график Гаусса
            for (double x = 7; x <= 17; x += 0.1)
            {
                double y = GaussPolynomial(x);
                gaussSeries.Points.AddXY(x, y);
            }

            chart1.Series.Add(gaussSeries);
            chart1.Series.Add(pointSeries);
        }

        private void DrawChart2()
        {
            chart2.Series.Clear();
            
            // Настройка области
            if (chart2.ChartAreas.Count == 0)
                chart2.ChartAreas.Add(new ChartArea());
            
            chart2.ChartAreas[0].AxisX.Title = "x";
            chart2.ChartAreas[0].AxisY.Title = "y";
            chart2.ChartAreas[0].AxisX.Minimum = 7;
            chart2.ChartAreas[0].AxisX.Maximum = 17;
            chart2.ChartAreas[0].AxisY.Minimum = -5;
            chart2.ChartAreas[0].AxisY.Maximum = 20;

            // Серия для графика Лагранжа
            Series lagrangeSeries = new Series("Метод Лагранжа");
            lagrangeSeries.ChartType = SeriesChartType.Line;
            lagrangeSeries.Color = Color.Green;
            lagrangeSeries.BorderWidth = 2;

            // Серия для точек
            Series pointSeries = new Series("Исходные точки");
            pointSeries.ChartType = SeriesChartType.Point;
            pointSeries.Color = Color.Red;
            pointSeries.MarkerSize = 8;
            pointSeries.MarkerStyle = MarkerStyle.Circle;

            // Добавляем точки
            for (int i = 0; i < xValues.Length; i++)
            {
                pointSeries.Points.AddXY(xValues[i], yValues[i]);
            }

            // Строим график Лагранжа
            for (double x = 7; x <= 17; x += 0.1)
            {
                double y = LagrangePolynomial(x);
                lagrangeSeries.Points.AddXY(x, y);
            }

            chart2.Series.Add(lagrangeSeries);
            chart2.Series.Add(pointSeries);
        }

        private void DrawChart3()
        {
            chart3.Series.Clear();
            
            // Настройка области
            if (chart3.ChartAreas.Count == 0)
                chart3.ChartAreas.Add(new ChartArea());
            
            chart3.ChartAreas[0].AxisX.Title = "x";
            chart3.ChartAreas[0].AxisY.Title = "y";
            chart3.ChartAreas[0].AxisX.Minimum = 7;
            chart3.ChartAreas[0].AxisX.Maximum = 17;
            chart3.ChartAreas[0].AxisY.Minimum = -5;
            chart3.ChartAreas[0].AxisY.Maximum = 20;

            // Серия для графика Гаусса
            Series gaussSeries = new Series("Метод Гаусса");
            gaussSeries.ChartType = SeriesChartType.Line;
            gaussSeries.Color = Color.Blue;
            gaussSeries.BorderWidth = 2;

            // Серия для графика Лагранжа
            Series lagrangeSeries = new Series("Метод Лагранжа");
            lagrangeSeries.ChartType = SeriesChartType.Line;
            lagrangeSeries.Color = Color.Green;
            lagrangeSeries.BorderWidth = 2;
            lagrangeSeries.BorderDashStyle = ChartDashStyle.Dash; // Пунктир для отличия

            // Серия для точек
            Series pointSeries = new Series("Исходные точки");
            pointSeries.ChartType = SeriesChartType.Point;
            pointSeries.Color = Color.Red;
            pointSeries.MarkerSize = 8;
            pointSeries.MarkerStyle = MarkerStyle.Circle;

            // Добавляем точки
            for (int i = 0; i < xValues.Length; i++)
            {
                pointSeries.Points.AddXY(xValues[i], yValues[i]);
            }

            // Строим графики
            for (double x = 7; x <= 17; x += 0.1)
            {
                double yGauss = GaussPolynomial(x);
                double yLagrange = LagrangePolynomial(x);
                
                gaussSeries.Points.AddXY(x, yGauss);
                lagrangeSeries.Points.AddXY(x, yLagrange);
            }

            chart3.Series.Add(gaussSeries);
            chart3.Series.Add(lagrangeSeries);
            chart3.Series.Add(pointSeries);
        }

        // Метод Гаусса
        private double GaussPolynomial(double x)
        {
            return a0 + a1 * x + a2 * x * x + a3 * x * x * x + a4 * x * x * x * x;
        }

        // Метод Лагранжа
        private double LagrangePolynomial(double x)
        {
            double result = 0;
            int n = xValues.Length;

            for (int i = 0; i < n; i++)
            {
                double term = yValues[i];
                
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        term *= (x - xValues[j]) / (xValues[i] - xValues[j]);
                    }
                }
                
                result += term;
            }
            
            return result;
        }

        // Проверка совпадения в узлах
        private void ValidateMethods()
        {
            Console.WriteLine("Проверка в узлах интерполяции:");
            Console.WriteLine("x\tf(x)\tГаусс\t\tЛагранж\t\tПогрешность Гаусса\tПогрешность Лагранжа");
            
            for (int i = 0; i < xValues.Length; i++)
            {
                double gauss = GaussPolynomial(xValues[i]);
                double lagrange = LagrangePolynomial(xValues[i]);
                double errorGauss = Math.Abs(gauss - yValues[i]);
                double errorLagrange = Math.Abs(lagrange - yValues[i]);
                
                Console.WriteLine($"{xValues[i]}\t{yValues[i]}\t{gauss:F6}\t{lagrange:F6}\t{errorGauss:E6}\t\t{errorLagrange:E6}");
            }
        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }
    }
}