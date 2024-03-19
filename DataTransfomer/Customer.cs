public class CustomerDataTransfomer
{
    public class Create
    {
        [JsonRequired]
        public string Name { get; set; }

        public string FullName { get; set; }

        [JsonRequired]
        public string Phone { get; set; }
    }
}
