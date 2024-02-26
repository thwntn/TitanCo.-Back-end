namespace ReferenceModel;

public class JwtPayload
{
    public string userId;
    public string role;
    public int nbf;
    public int exp;
    public int iat;
    public string iss;
    public string aud;
}
