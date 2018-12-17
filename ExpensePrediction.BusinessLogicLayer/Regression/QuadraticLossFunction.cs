using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensePrediction.BusinessLogicLayer.Regression
{
    class QuadraticLossFunction
    {
        public double Loss(IEnumerable<(double, double)> values)
        {
            //TODO consider multithreading? divide values into four(?) parts and do them using tasks? or not
            var loss = 0.0d;
            foreach(var tuple in values)
            {
                var difference = tuple.Item1 - tuple.Item2;
                loss += difference * difference;
            }

            return loss;
        }
    }
}
