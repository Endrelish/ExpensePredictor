namespace ExpensePrediction.DataTransferObjects.User
{
    public class UserEditDto : IDataTransferObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}