using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensePrediction.DataTransferObjects
{
    public class PredictionResultDto : IDataTransferObject
    {
        public double FirstMonthValue { get; set; }
        public double SecondMonthValue { get; set; }
        public double ThirdMonthValue { get; set; }
    }
}
