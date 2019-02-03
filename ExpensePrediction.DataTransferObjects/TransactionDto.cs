using System;

namespace ExpensePrediction.DataTransferObjects
{
    public class TransactionDto : IDataTransferObject
    {
        public string Id { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string CategoryId { get; set; }
        public string Description { get; set; }
    }
}