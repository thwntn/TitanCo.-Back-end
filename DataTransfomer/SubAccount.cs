namespace ReferenceDatatransfomer;

public class ExtraDatatransformer
{
    public class Create
    {
        [JsonRequired]
        public string UserName { get; set; }

        [JsonRequired]
        public string Password { get; set; }

        [JsonRequired]
        public string Email { get; set; }

        [JsonRequired]
        public string Name { get; set; }
    }

    public class Signin
    {
        [JsonRequired]
        public string UserName { get; set; }

        [JsonRequired]
        public string Password { get; set; }
    }
}
