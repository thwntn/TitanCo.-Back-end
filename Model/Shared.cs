namespace ReferenceModel
{
    public class MShare
    {
        public class CompressItem(string name, string url, long size, DateTime created)
        {
            public string name = name;
            public DateTime cretaed = created;
            public string url = url;
            public long size = size;
        }
    }
}
