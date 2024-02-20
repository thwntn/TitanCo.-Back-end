namespace ReferenceService;

public class AuthService(
    DatabaseContext databaseContext,
    ISecurity securityService,
    IMail mailService,
    IGoogle googleService
) : IAuth
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly ISecurity _securityService = securityService;
    private readonly IMail _mailService = mailService;
    private readonly IGoogle _googleService = googleService;

    public string Code(int userId, string code)
    {
        var user =
            _databaseContext.User.FirstOrDefault(user => user.Id == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);
        if (user.Code != code)
            return MessageDefine.CONFIRM_CODE_NOT_SUCCESS;

        user.Status = UserStatus.Open;
        _databaseContext.Update(user);
        _databaseContext.SaveChanges();

        return MessageDefine.CONFIRM_CODE_SUCCESS;
    }

    public User Signup(AuthDataTransformer.Signup signup)
    {
        bool existNameOrEmail = _databaseContext
            .User
            .Any(user => user.Name == signup.name || user.Email == signup.username);
        if (existNameOrEmail)
            throw new HttpException(400, MessageDefine.USERNAME_EMAIL_IS_EXIST);

        string md5Password = Cryptography.Md5(signup.password);
        string code = Cryptography.RandomCode().ToString();
        User user =
            new()
            {
                Password = md5Password,
                Name = signup.name,
                Avatar = string.Empty,
                Email = signup.username,
                Code = code,
                Status = UserStatus.Valid,
                CoverPicture = string.Empty
            };

        _databaseContext.Add(user);
        _databaseContext.SaveChanges();

        _mailService.SendCode(user.Email, code);
        return user;
    }

    public MLogin.Info Signin(AuthDataTransformer.Signin signin)
    {
        string md5Password = Cryptography.Md5(signin.password);
        var user =
            _databaseContext.User.FirstOrDefault(user => user.Email == signin.username && user.Password == md5Password)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        MLogin.Info info = NewtonsoftJson.Map<MLogin.Info>(user);
        if (user.Status == UserStatus.Open)
            info.token = _securityService.GenerateToken(user.Id.ToString());
        else
        {
            string code = Cryptography.RandomCode().ToString();
            user.Code = code;
            _databaseContext.Update(user);
            _databaseContext.SaveChanges();
            _mailService.SendCode(user.Email, code);
        }

        info.Password = string.Empty;
        return info;
    }

    public async Task<User> LoginGoogle(string authCode)
    {
        MGoogle.AccessTokenResponse fromGg = await _googleService.GetAccessToken(authCode);

        if (fromGg is null)
            return null;

        MGoogle.AccessTokenResponse objectToken = NewtonsoftJson.Map<MGoogle.AccessTokenResponse>(fromGg);
        MGoogle.GetProfileResponse getProfileResponse = await _googleService.GetProfile(objectToken.access_token);

        if (getProfileResponse is null)
            return null;

        MGoogle.GetProfileResponse info = NewtonsoftJson.Map<MGoogle.GetProfileResponse>(getProfileResponse);
        User user = _databaseContext.User.FirstOrDefault(user => user.Google.Sub == info.sub);
        User handler = InsertInfo(info);
        return handler;
    }

    private User InsertInfo(MGoogle.GetProfileResponse info)
    {
        User user =
            new()
            {
                Google = new() { Sub = info.sub, Picture = info.picture },
                Password = string.Empty,
                Type = UserType.Google,
                Name = info.name,
                Email = string.Empty,
                Status = UserStatus.Open,
                Avatar = info.picture
            };

        _databaseContext.Add(user);
        _databaseContext.SaveChanges();
        return user;
    }
}
