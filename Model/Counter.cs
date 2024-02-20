namespace ReferenceModel;

public class Counter<T>(T key, long count, long size)
{
    public T key = key;
    public long count = count;
    public long size = size;
}
