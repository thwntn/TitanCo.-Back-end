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

    public static T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json);

    public static T Map<T>(object data) => Deserialize<T>(Serialize(data));
}
