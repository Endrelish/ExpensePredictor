namespace ExpensePrediction.DataTransferObjects.User
{
    public class LoginDto : IDataTransferObject
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}