namespace ReferenceModel;

public class MStogare
{
    public class StogareWithCounter : Stogare
    {
        public long counter;
        public long counterSize;
    }

    public class StogareWithThumbnail(string key, string fileName, string path, long size, string thumbnail)
        : MStream.Save(key, fileName, path, size)
    {
        private readonly string _thumbnail = thumbnail;

        public string GetThumbnail()
        {
            return _thumbnail;
        }
    }

    public class StogareWithLevel : Stogare
    {
        public int level;
    }
}
