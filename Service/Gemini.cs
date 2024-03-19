namespace ReferenceService;

public class GeminiService() : IGemini
{
    private readonly HttpClient _client = new();
    private readonly string _GeminiUrl = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.GeminiUrl));
    private readonly string _GeminiKey = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.GeminiKey));

    public async Task<IEnumerable<MGemini.Response.Text>> Chat(string input)
    {
        HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, $"{_GeminiUrl}{_GeminiKey}")
        {
            Content = new StringContent(
                NewtonsoftJson.Serialize(new MGemini.Data(input)),
                Encoding.UTF8,
                "application/json"
            )
        };
        HttpResponseMessage httpResponseMessage = await _client.SendAsync(message);
        string content = await httpResponseMessage.Content.ReadAsStringAsync();
        Logger.Json(content);

        MGemini.Response result = NewtonsoftJson.Deserialize<MGemini.Response>(content);
        return result.candidates.FirstOrDefault().content.parts;
    }
}
