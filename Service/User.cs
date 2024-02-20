namespace ReferenceService;

public class UserService(DatabaseContext databaseContext, ISecurity securityService) : IUser
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly ISecurity _securityService = securityService;

    public User Info(int userId)
    {
        var user =
            _databaseContext.User.FirstOrDefault(item => item.Id == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        user.Password = string.Empty;
        return user;
    }

    public User Update(int userId, UserDataTransfromer.Update update)
    {
        var user =
            _databaseContext.User.FirstOrDefault(record => record.Id == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        user.Avatar = update.avatar;
        user.Name = update.name;

        _databaseContext.Update(user);
        user.Password = string.Empty;

        return user;
    }

    public async Task<MLogin.Info> ChangeAvatar(IFormFile file, int userId)
    {
        var user =
            _databaseContext.User.FirstOrDefault(item => item.Id == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        MStream.Save save = await Reader.Save(file, string.Empty);
        user.Avatar = Reader.CreateStogare(save.GetPath());

        MLogin.Info info = NewtonsoftJson.Map<MLogin.Info>(user);
        info.token = _securityService.GenerateToken(user.Id.ToString());

        _databaseContext.Update(user);
        _databaseContext.SaveChanges();

        return info;
    }

    public async Task<MLogin.Info> ChangeCoverPicture(IFormFile file, int userId)
    {
        var user =
            _databaseContext.User.FirstOrDefault(item => item.Id == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        MStream.Save save = await Reader.Save(file, string.Empty);
        user.CoverPicture = Reader.CreateStogare(save.GetPath());

        MLogin.Info info = NewtonsoftJson.Map<MLogin.Info>(user);
        info.token = _securityService.GenerateToken(user.Id.ToString());

        _databaseContext.Update(user);
        _databaseContext.SaveChanges();

        return info;
    }
}
