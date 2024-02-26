namespace ReferenceService;

public class AuthService(
    DatabaseContext databaseContext,
    ISecurity securityService,
    IMail mailService,
    IGoogle googleService,
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager
) : IAuth
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly ISecurity _securityService = securityService;
    private readonly IMail _mailService = mailService;
    private readonly IGoogle _googleService = googleService;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;

    public string Code(string profileId, string code)
    {
        var user =
            _databaseContext.Profile.FirstOrDefault(user => user.Id == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);
        if (user.Code != code)
            return MessageDefine.CONFIRM_CODE_NOT_SUCCESS;

        user.Status = UserStatus.Open;
        _databaseContext.Update(user);
        _databaseContext.SaveChanges();

        return MessageDefine.CONFIRM_CODE_SUCCESS;
    }

    public async Task<Profile> Signup(AuthDataTransformer.Signup signup)
    {
        string code = Cryptography.RandomCode().ToString();
        Profile profile =
            new()
            {
                Id = Cryptography.RandomGuid(),
                Name = signup.name,
                Avatar = string.Empty,
                Email = signup.username,
                Code = code,
                Status = UserStatus.Valid,
                CoverPicture = string.Empty,
            };

        // @Create Identity
        IdentityUser identityUser = new() { Email = profile.Email, UserName = profile.Email, };
        var create = await _userManager.CreateAsync(identityUser, signup.password);
        if (create.Succeeded is false)
            throw new HttpException(400, create);
        profile.UserId = identityUser.Id;

        // @Add Profile
        _databaseContext.Add(profile);
        _databaseContext.SaveChanges();
        _mailService.SendCode(profile.Email, code);

        return profile;
    }

    public async Task<MLogin.Info> Signin(AuthDataTransformer.Signin signin)
    {
        string hashPassword = Cryptography.Md5(signin.password);
        var user = await _signInManager.PasswordSignInAsync(signin.username, signin.password, false, false);

        if (user.Succeeded is false)
            throw new HttpException(401, MessageDefine.NOT_FOUND_USER);

        var profile =
            _databaseContext.Profile.FirstOrDefault(profile => profile.Email == signin.username)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        MLogin.Info info = NewtonsoftJson.Map<MLogin.Info>(profile);
        if (profile.Status == UserStatus.Open)
            info.token = _securityService.GenerateToken(profile.Id.ToString());
        else
        {
            string code = $"{Cryptography.RandomCode()}";
            profile.Code = code;
            _databaseContext.Update(user);
            _databaseContext.SaveChanges();
            _mailService.SendCode(profile.Email, code);
        }
        return info;
    }

    public async Task<Profile> LoginGoogle(string authCode)
    {
        MGoogle.AccessTokenResponse fromGg = await _googleService.GetAccessToken(authCode);

        if (fromGg is null)
            return null;

        MGoogle.AccessTokenResponse objectToken = NewtonsoftJson.Map<MGoogle.AccessTokenResponse>(fromGg);
        MGoogle.GetProfileResponse getProfileResponse = await _googleService.GetProfile(objectToken.access_token);

        if (getProfileResponse is null)
            return null;

        MGoogle.GetProfileResponse info = NewtonsoftJson.Map<MGoogle.GetProfileResponse>(getProfileResponse);
        Profile profile = _databaseContext.Profile.FirstOrDefault(profile => profile.Google.Sub == info.sub);
        Profile handler = InsertInfo(info);
        return handler;
    }

    private Profile InsertInfo(MGoogle.GetProfileResponse info)
    {
        Profile profile =
            new()
            {
                Google = new() { Sub = info.sub, Picture = info.picture },
                Type = UserType.Google,
                Name = info.name,
                Email = string.Empty,
                Status = UserStatus.Open,
                Avatar = info.picture
            };

        _databaseContext.Add(profile);
        _databaseContext.SaveChanges();
        return profile;
    }
}
