namespace ReferenceFeature;

public class NewtonsoftJson
{
    public static string Serialize(object obj) =>
        JsonConvert.SerializeObject(
            obj,
            Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            }
        );

    public static T Deserialize<T>(string json)
        where T : new() => JsonConvert.DeserializeObject<T>(json);

    public static T Map<T>(object data)
        where T : new() => Deserialize<T>(Serialize(data));
}
