namespace ReferenceInterface;

public interface IGoogle
{
    Task<MGoogle.AccessTokenResponse> GetAccessToken(string authCode);
    Task<MGoogle.ProfileResponse> Profile(string accessToken);
}
