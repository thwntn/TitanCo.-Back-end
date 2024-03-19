namespace ReferenceInterface;

public interface IAuth
{
    Task<Account> Signup(AuthDataTransformer.Signup signup);
    bool VerifyEmail(AuthDataTransformer.VerifyEmail verifyEmail);
    Task<Account> SigninWithPassword(AuthDataTransformer.Signin signin);
    Account ConfirmCode(Guid accountId, string code);
    Task<Profile> LoginGoogle(string authCode);
    string ChangePassword(AuthDataTransformer.ChangePassword changePassword);
    Task<string> ResetPassword(string email);
}
