namespace ReferenceInterface;

public interface IJwt
{
    string GenerateToken(Guid profileId, Guid accountId, Guid parentId);
    void SetCookie(IResponseCookies responseCookies, object data);
    Infomation ReadToken(HttpRequest httpRequest);
    IEnumerable<Account> AccountSystem();
    Infomation Infomation();
    Account Account();
}
