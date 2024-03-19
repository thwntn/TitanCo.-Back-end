namespace ReferenceDatatransfomer;

public class ProfileDataTransfromer
{
    public class Update
    {
        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public string Phone { get; set; }

        [JsonRequired]
        public string Address { get; set; }
    }
}
