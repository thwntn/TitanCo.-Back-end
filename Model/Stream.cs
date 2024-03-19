namespace ReferenceModel;

public class MStream
{
    public class Blob(string key, string fileName, string path, long size)
    {
        private readonly string _key = key;
        private readonly string _fileName = fileName;
        private string _path = path;
        private readonly long _size = size;

        public string GetKey()
        {
            return _key;
        }

        public string GetFileName()
        {
            return _fileName;
        }

        public string GetPath()
        {
            return _path;
        }

        public long GetSize()
        {
            return _size;
        }

        public void SetPath(string path)
        {
            _path = path;
        }
    }
}
