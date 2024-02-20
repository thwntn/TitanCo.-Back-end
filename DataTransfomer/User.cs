namespace ReferenceDatatransfomer;

public class UserDataTransfromer
{
    public class Update
    {
        [JsonRequired]
        public string name;

        [JsonRequired]
        public string avatar;
    }
}
