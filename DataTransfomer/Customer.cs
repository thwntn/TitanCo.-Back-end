public class CustomerDataTransfomer
{
    public class Create
    {
        [JsonRequired]
        public string name;

        [JsonRequired]
        public string phone;
    }
}
