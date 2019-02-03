using ExpensePrediction.BusinessLogicLayer.Regression.Model;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Linq;
using ExpensePrediction.Exceptions;

namespace ExpensePrediction.BusinessLogicLayer.Regression
{
    public class RegressionModel
    {
        public double[] Coefficients { get; }
        public RegressionModel(int predictors)
        {
            Coefficients = new double[predictors];
        }

        public double PredictTarget(double[] predictors)
        {
            if (predictors.Length != Coefficients.Length) throw new RegressionException();

            var prediction = 0.0d;
            for(int i = 0; i < predictors.Length; i++)
            {
                prediction += predictors[i] * Coefficients[i];
            }

            return prediction;
        }

        public void CalculateCoefficients(DataSet dataSet)
        {
            var predictors = ExtractPredictorsMatrix(dataSet);
            var targets = ExtractTargetsMatrix(dataSet);
            var predictorsTransposed = predictors.Transpose();

            var coefficients = predictorsTransposed
                .Multiply(predictors)
                .Inverse()
                .Multiply(predictorsTransposed)
                .Multiply(targets);

            int i = 0;
            foreach(var coefficient in coefficients.Enumerate())
            {
                Coefficients[i++] = coefficient;
            }
        }

        private Matrix ExtractPredictorsMatrix(DataSet dataSet)
        {
            var predictors = new DenseMatrix(dataSet.Rows, dataSet.Columns);

            var i = 0;
            foreach(var tuple in dataSet.Tuples)
            {
                if(tuple.Target.HasValue)
                    predictors.SetRow(i++, tuple.Predictors);
            }

            return predictors;
        }

        private Matrix ExtractTargetsMatrix(DataSet dataSet)
        {
            var targets = new DenseMatrix(dataSet.Rows, 1);
            var i = 0;
            foreach(var tuple in dataSet.Tuples)
            {
                if(tuple.Target.HasValue)
                    targets.SetRow(i++, new[] { tuple.Target.Value });
            }

            return targets;
        }
    }
}
