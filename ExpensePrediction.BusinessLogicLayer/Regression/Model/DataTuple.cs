using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpensePrediction.BusinessLogicLayer.Regression.Model
{
    public class DataTuple
    {
        public double? Target { get; }
        public double[] Predictors { get; }

        public DataTuple(double[] predictors, double? target = null)
        {
            Target = target;
            Predictors = predictors;
        }
    }
}
