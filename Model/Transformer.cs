namespace ReferenceModel;

public class MTransform<T>(T data)
{
    public readonly List<string> errors = [];
    public readonly T data = data;
}
