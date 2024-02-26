namespace ReferenceInterface;

public interface IAuth
{
    string Code(string profileId, string code);
    Task<Profile> Signup(AuthDataTransformer.Signup signup);
    Task<MLogin.Info> Signin(AuthDataTransformer.Signin signin);
    Task<Profile> LoginGoogle(string authCode);
}
