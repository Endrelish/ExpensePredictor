using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpensePrediction.BusinessLogicLayer.Regression.Model
{
    public class DataSet
    {
        public IEnumerable<DataTuple> Tuples { get; private set; }
        public int Rows { get; }
        public int Columns { get; }

        public DataSet(double?[] targets, double[][] predictorSets)
        {
            if (targets.Length != predictorSets.Length)
                throw new Exception("Number of targets does not equal the number of predictor sets."); //TODO custom exceptions
            var tuples = new List<DataTuple>();

            var predictors = predictorSets[0].Length;
            for(int i = 0; i < predictorSets.Length; i++)
            {
                if (predictorSets[i].Length != predictors)
                    throw new Exception("Number of predictors must be the same for all tuples."); //TODO custom exceptions

                tuples.Add(new DataTuple(predictorSets[i], targets[i]));
            }

            Tuples = tuples.AsReadOnly();
            Rows = tuples.Count;
            Columns = predictors;
        }
    }
}
