using ExpensePrediction.BusinessLogicLayer.Regression;
using System;

namespace TestFront
{
    internal class Program
    {
        private static (double[], double?[], double[][]) RandomModel()
        {
            var rnd = new Random();
            var predictorsNumber = rnd.Next(8) + 2;
            var coefficients = new double[predictorsNumber];
            for(int i = 0; i < predictorsNumber; i++)
            {
                coefficients[i] = rnd.NextDouble() * 10;
            }

            var predictors = new double[10000][];
            var targets = new double?[10000];
            for(int i = 0; i < 10000; i++)
            {
                predictors[i] = new double[predictorsNumber];
                var target = 0.0d;
                for(int j = 0; j < predictorsNumber; j++)
                {
                    predictors[i][j] = rnd.Next(20);
                    target += predictors[i][j] * coefficients[j];
                }
                target += target * ((rnd.NextDouble() - 0.5d) * 0.1d);
                targets[i] = target;
            }

            return (coefficients, targets, predictors);
        }
        private static void Main(string[] args)
        {
            var randomModel = RandomModel();
            var coefficients = randomModel.Item1;
            var targets = randomModel.Item2;
            var predictors = randomModel.Item3;
            var model = new RegressionModel(coefficients.Length);

            model.CalculateCoefficients(new ExpensePrediction.BusinessLogicLayer.Regression.Model.DataSet(targets, predictors));

            for(int i = 0; i < coefficients.Length; i++)
            {
                Console.WriteLine("{0} - {1}", coefficients[i].ToString("N5"), model.Coefficients[i].ToString("N5"));
            }

            Console.ReadKey();
        }
    }
}