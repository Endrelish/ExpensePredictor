using ExpensePrediction.BusinessLogicLayer.Regression;
using System;

namespace TestFront
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var model = new RegressionModel(2);
            var targets = new double?[] { 7.0d, 9.0d, 20.0d, 17.0d, 74.0d };
            var predictors = new double[][]
            {
                new [] { 1.0d, 1.0d },
                new [] { 2.0d, 1.0d },
                new [] { 5.0d, 2.0d },
                new [] { 6.0d, 1.0d },
                new [] { 12.0d, 10.0d }
            };

            model.CalculateCoefficients(new ExpensePrediction.BusinessLogicLayer.Regression.Model.DataSet(targets, predictors));
            foreach (var coeff in model.Coefficients)
                Console.WriteLine(coeff);

            Console.ReadKey();
        }
    }
}