namespace ReferenceModel;

public class MHome
{
    public class Counter(string name)
    {
        public string name = name;
        public int quanlity = 0;
        public long size = 0;
        public long percent = 0;
    }

    public class Info(long totalSize, long totalFile, List<Counter> counter, Settings setting)
    {
        public Settings setting = setting;
        public List<Counter> counter = counter;
        public long totalFile = totalFile;
        public long totalSize = totalSize;
    }

    public class Settings(long maxSize)
    {
        public long maxSize = maxSize;
    }
}
