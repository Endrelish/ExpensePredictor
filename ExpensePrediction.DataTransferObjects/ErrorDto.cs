namespace ExpensePrediction.DataTransferObjects
{
    public class ErrorDto : IDataTransferObject
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}