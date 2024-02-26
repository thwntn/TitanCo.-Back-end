namespace ReferenceDatatransfomer;

public class ProfileDataTransfromer
{
    public class Update
    {
        [JsonRequired]
        public string name;

        [JsonRequired]
        public string avatar;
    }
}
