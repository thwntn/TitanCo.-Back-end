namespace ReferenceDatatransfomer;

public class AuthDataTransformer
{
    public class Google
    {
        [JsonRequired]
        public string username;

        [JsonRequired]
        public string password;
    }

    public class Code
    {
        [JsonRequired]
        public string userId;

        [JsonRequired]
        public string code;
    }

    public class Signin
    {
        [JsonRequired]
        public string username;

        [JsonRequired]
        public string password;
    }

    public class Signup
    {
        [JsonRequired]
        public string name;

        [JsonRequired]
        public string username;

        [JsonRequired]
        public string password;
    }
}
