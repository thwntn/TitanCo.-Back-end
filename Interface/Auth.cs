namespace ReferenceInterface;

public interface IAuth
{
    string Code(int userId, string code);
    User Signup(AuthDataTransformer.Signup signup);
    MLogin.Info Signin(AuthDataTransformer.Signin signin);
    Task<User> LoginGoogle(string authCode);
}
