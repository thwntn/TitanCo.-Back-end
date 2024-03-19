namespace ReferenceDatatransfomer;

public class PlanningDataTransformer
{
    public class Create
    {
        [JsonRequired]
        public string WeekOfYear { get; set; }

        [JsonRequired]
        public string DateTime { get; set; }

        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public string Hour { get; set; }

        [JsonRequired]
        public string From { get; set; }

        [JsonRequired]
        public string To { get; set; }

        [JsonRequired]
        public string Day { get; set; }

        [JsonRequired]
        public string Color { get; set; }

        [JsonRequired]
        public string SetNotification { get; set; }

        [JsonRequired]
        public string SetEmail { get; set; }
    }
}
