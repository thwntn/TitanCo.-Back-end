public class CustomerDataTransfomer
{
    public class Create
    {
        [JsonRequired]
        public string name;

        public string fullName;

        [JsonRequired]
        public string phone;
    }
}
