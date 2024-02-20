namespace ReferenceInterface;

public interface ISecurity
{
    string GenerateToken(string userId);
    void SetCookie(IResponseCookies responseCookies, object data);
    ReferenceModel.JwtPayload ReadToken(HttpRequest httpRequest);
}
