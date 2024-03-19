namespace ReferenceModel;

public class MGoogle
{
    public class GetAccessToken(
        string code,
        string clientId,
        string clientSecrect,
        string redirectUri,
        string grantType
    )
    {
        public string client_secret = clientSecrect;
        public string redirect_uri = redirectUri;
        public string code = code;
        public string client_id = clientId;
        public string grant_type = grantType;
    }

    public class AccessTokenResponse
    {
        public string access_token;
        public string expires_in;
        public string refresh_token;
        public string scope;
        public string token_type;
        public string id_token;
    }

    public class ProfileResponse
    {
        public string sub;
        public string name;
        public string given_name;
        public string family_name;
        public string picture;
        public string locale;
    }
}
