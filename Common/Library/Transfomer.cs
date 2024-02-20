namespace ReferenceFeature;

public class Transformer
{
    public static MTransform<T> Map<T>(object data)
        where T : new()
    {
        System.Reflection.FieldInfo[] fields = typeof(T).GetFields();
        Dictionary<string, object> request = NewtonsoftJson.Map<Dictionary<string, object>>(data);
        MTransform<T> result = new(NewtonsoftJson.Map<T>(data));
        fields
            .ToList()
            .ForEach(i =>
            {
                if (request.ContainsKey(i.Name) is false)
                    result.errors.Add(i.Name);
            });
        return result;
    }
}
