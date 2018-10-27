namespace AuthWebApi.Dto
{
    public class RegisterDto : UserDataDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}