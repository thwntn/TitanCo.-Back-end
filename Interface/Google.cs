namespace ReferenceInterface;

public interface IGoogle
{
    Task<MGoogle.AccessTokenResponse> GetAccessToken(string authCode);
    Task<MGoogle.GetProfileResponse> GetProfile(string accessToken);
}
