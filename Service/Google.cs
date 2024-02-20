namespace ReferenceService;

public class GoogleService : IGoogle
{
    public async Task<MGoogle.AccessTokenResponse> GetAccessToken(string authCode)
    {
        string url = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.OauthHost)) + "/token";
        MGoogle.GetAccessToken content =
            new(
                authCode,
                Environment.GetEnvironmentVariable(nameof(EnvironmentKey.ClientId)),
                Environment.GetEnvironmentVariable(nameof(EnvironmentKey.SecretId)),
                Environment.GetEnvironmentVariable(nameof(EnvironmentKey.GoogleRedirect)),
                Environment.GetEnvironmentVariable(nameof(EnvironmentKey.GrandType))
            );

        FormUrlEncodedContent f = new(NewtonsoftJson.Map<Dictionary<string, string>>(content));
        HttpResponseMessage rs = await new HttpClient().PostAsync(url, f);

        string s = await rs.Content.ReadAsStringAsync();
        MGoogle.AccessTokenResponse acessrs = NewtonsoftJson.Deserialize<MGoogle.AccessTokenResponse>(s);

        if (acessrs.access_token is null)
            return null;

        return acessrs;
    }

    public async Task<MGoogle.GetProfileResponse> GetProfile(string accessToken)
    {
        string url =
            Environment.GetEnvironmentVariable(nameof(EnvironmentKey.ProfileHost))
            + $"/oauth2/v3/userinfo?access_token={accessToken}";
        Logger.Json(url);
        HttpResponseMessage rs = await new HttpClient().GetAsync(url);

        string s = await rs.Content.ReadAsStringAsync();
        MGoogle.GetProfileResponse info = NewtonsoftJson.Deserialize<MGoogle.GetProfileResponse>(s);

        if (info.sub is null)
            return null;

        return info;
    }
}
