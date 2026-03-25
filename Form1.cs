using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CompMathLab3
{
    public partial class Form1 : Form
    {
        private double[] xValues = { 8, 10, 12, 14, 16 };
        private double[] yValues = { 4, 9, 5, 1, 16 };
        private double[] interpolationCoeffs;
        private double[] smoothingCoeffs;

        public Form1()
        {
            InitializeComponent();
            CalculateInterpolationCoefficients();
            CalculateSmoothingCoefficients();
            DrawAllGraphs();
        }

        private void CalculateInterpolationCoefficients()
        {
            float[] coeffs = CompMathLab2.GaussianMethod.SolveInterpolationSystem(xValues, yValues);
            interpolationCoeffs = Array.ConvertAll(coeffs, item => (double)item);
        }

        private void CalculateSmoothingCoefficients()
        {
            int degree = 2; // Степень для сглаживающего метода
            float[] xFloat = Array.ConvertAll(xValues, item => (float)item);
            float[] yFloat = Array.ConvertAll(yValues, item => (float)item);

            var leastSquares = new CompMathLab2.LeastSquaresMethod(xFloat, yFloat, degree);
            float[] coeffs = leastSquares.Run();
            smoothingCoeffs = Array.ConvertAll(coeffs, item => (double)item);
        }

        private void DrawAllGraphs()
        {
            DrawChart1();
            DrawChart2();
            DrawChart3();
            DrawChart4();
        }

        private void SetupChartArea(Chart chart)
        {
            if (chart.ChartAreas.Count == 0)
                chart.ChartAreas.Add(new ChartArea());

            chart.ChartAreas[0].AxisX.Title = "x";
            chart.ChartAreas[0].AxisY.Title = "y";
            chart.ChartAreas[0].AxisX.Minimum = 7;
            chart.ChartAreas[0].AxisX.Maximum = 17;
            chart.ChartAreas[0].AxisY.Minimum = -5;
            chart.ChartAreas[0].AxisY.Maximum = 20;
        }

        private void AddPointSeries(Chart chart)
        {
            Series pointSeries = new Series("Исходные точки");
            pointSeries.ChartType = SeriesChartType.Point;
            pointSeries.Color = Color.Red;
            pointSeries.MarkerSize = 8;
            pointSeries.MarkerStyle = MarkerStyle.Circle;

            for (int i = 0; i < xValues.Length; i++)
            {
                pointSeries.Points.AddXY(xValues[i], yValues[i]);
            }

            chart.Series.Add(pointSeries);
        }

        private void DrawChart1()
        {
            chart1.Series.Clear();
            SetupChartArea(chart1);

            Series interpolationSeries = new Series("Интерполяционный полином (решение системы 3.1)");
            interpolationSeries.ChartType = SeriesChartType.Line;
            interpolationSeries.Color = Color.Blue;
            interpolationSeries.BorderWidth = 2;

            for (double x = 7; x <= 17; x += 0.1)
            {
                double y = InterpolationPolynomial(x);
                interpolationSeries.Points.AddXY(x, y);
            }

            chart1.Series.Add(interpolationSeries);
            AddPointSeries(chart1);
        }

        private void DrawChart2()
        {
            chart2.Series.Clear();
            SetupChartArea(chart2);

            Series lagrangeSeries = new Series("Метод Лагранжа");
            lagrangeSeries.ChartType = SeriesChartType.Line;
            lagrangeSeries.Color = Color.Green;
            lagrangeSeries.BorderWidth = 2;

            for (double x = 7; x <= 17; x += 0.1)
            {
                double y = LagrangePolynomial(x);
                lagrangeSeries.Points.AddXY(x, y);
            }

            chart2.Series.Add(lagrangeSeries);
            AddPointSeries(chart2);
        }

        private void DrawChart3()
        {
            chart3.Series.Clear();
            SetupChartArea(chart3);

            Series interpolationSeries = new Series("Интерполяционный полином (решение системы 3.1)");
            interpolationSeries.ChartType = SeriesChartType.Line;
            interpolationSeries.Color = Color.Blue;
            interpolationSeries.BorderWidth = 2;

            Series lagrangeSeries = new Series("Метод Лагранжа");
            lagrangeSeries.ChartType = SeriesChartType.Line;
            lagrangeSeries.Color = Color.Green;
            lagrangeSeries.BorderWidth = 2;
            lagrangeSeries.BorderDashStyle = ChartDashStyle.Dash;

            Series smoothingSeries = new Series("Сглаживающий метод (МНК)");
            smoothingSeries.ChartType = SeriesChartType.Line;
            smoothingSeries.Color = Color.Orange;
            smoothingSeries.BorderWidth = 2;

            for (double x = 7; x <= 17; x += 0.1)
            {
                interpolationSeries.Points.AddXY(x, InterpolationPolynomial(x));
                lagrangeSeries.Points.AddXY(x, LagrangePolynomial(x));
                smoothingSeries.Points.AddXY(x, SmoothingPolynomial(x));
            }

            chart3.Series.Add(interpolationSeries);
            chart3.Series.Add(lagrangeSeries);
            chart3.Series.Add(smoothingSeries);
            AddPointSeries(chart3);
        }

        private void DrawChart4()
        {
            chart4.Series.Clear();
            SetupChartArea(chart4);

            Series smoothingSeries = new Series("Сглаживающий метод (МНК)");
            smoothingSeries.ChartType = SeriesChartType.Line;
            smoothingSeries.Color = Color.Orange;
            smoothingSeries.BorderWidth = 2;

            for (double x = 7; x <= 17; x += 0.1)
            {
                smoothingSeries.Points.AddXY(x, SmoothingPolynomial(x));
            }

            chart4.Series.Add(smoothingSeries);
            AddPointSeries(chart4);
        }

        private double InterpolationPolynomial(double x)
        {
            if (interpolationCoeffs == null) return 0;

            double result = 0;
            for (int i = 0; i < interpolationCoeffs.Length; i++)
            {
                result += interpolationCoeffs[i] * Math.Pow(x, i);
            }
            return result;
        }

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
                        term *= (x - xValues[j]) / (xValues[i] - xValues[j]);
                }
                result += term;
            }
            return result;
        }

        private double SmoothingPolynomial(double x)
        {
            if (smoothingCoeffs == null) return 0;

            double result = 0;
            for (int i = 0; i < smoothingCoeffs.Length; i++)
                result += smoothingCoeffs[i] * Math.Pow(x, i);

            return result;
        }
    }
}