using System;

namespace ExpensePrediction.DataTransferObjects.User
{
    public class TokenDto : IDataTransferObject
    {
        public string UserId { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Token { get; set; }
    }
}