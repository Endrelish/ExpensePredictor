namespace AuthWebApi.Dto
{
    public class PasswordChangeDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordRepeated { get; set; }
    }
}