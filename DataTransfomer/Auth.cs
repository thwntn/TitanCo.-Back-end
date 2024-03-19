namespace ReferenceDatatransfomer;

public class AuthDataTransformer
{
    public class Google
    {
        [JsonRequired]
        public string Email { get; set; }

        [JsonRequired]
        public string Password { get; set; }
    }

    public class ConfirmCode
    {
        [JsonRequired]
        public Guid AccountId { get; set; }

        [JsonRequired]
        public string Code { get; set; }
    }

    public class ChangePassword
    {
        [JsonRequired]
        public string Email { get; set; }

        [JsonRequired]
        public string Password { get; set; }

        [JsonRequired]
        public string NewPassword { get; set; }

        [JsonRequired]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPassword
    {
        [JsonRequired]
        public string Email { get; set; }
    }

    public class VerifyEmail
    {
        [JsonRequired]
        public string Email { get; set; }
    }

    public class Signin
    {
        [JsonRequired]
        public string Email { get; set; }

        [JsonRequired]
        public string Password { get; set; }
    }

    public class Signup
    {
        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public string Email { get; set; }

        [JsonRequired]
        public string Password { get; set; }
    }
}
