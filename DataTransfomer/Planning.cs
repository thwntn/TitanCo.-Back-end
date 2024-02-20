namespace ReferenceDatatransfomer;

public class PlanningDataTransformer
{
    public class Create
    {
        [JsonRequired]
        public string weekOfYear;

        [JsonRequired]
        public string dateTime;

        [JsonRequired]
        public string name;

        [JsonRequired]
        public string hour;

        [JsonRequired]
        public string from;

        [JsonRequired]
        public string to;

        [JsonRequired]
        public string day;

        [JsonRequired]
        public string color;

        [JsonRequired]
        public string setNotification;

        [JsonRequired]
        public string setEmail;
    }
}
