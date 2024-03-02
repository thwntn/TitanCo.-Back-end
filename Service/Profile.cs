namespace ReferenceService;

public class ProfileService(DatabaseContext databaseContext, IJwt jwtService) : IProfile
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IJwt _jwtService = jwtService;

    public List<Profile> List()
    {
        var profiles = _databaseContext.Profile.ToList();
        return profiles;
    }

    public Profile Info(string profileId)
    {
        var profile =
            _databaseContext.Profile.FirstOrDefault(profile => profile.Id == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        return profile;
    }

    public Profile Update(string profileId, ProfileDataTransfromer.Update update)
    {
        var profile =
            _databaseContext.Profile.FirstOrDefault(record => record.Id == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        profile.Avatar = update.avatar;
        profile.Name = update.name;

        _databaseContext.Update(profile);

        return profile;
    }

    public async Task<MLogin.Info> ChangeAvatar(IFormFile file, string profileId)
    {
        var profile =
            _databaseContext.Profile.FirstOrDefault(profile => profile.Id == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        MStream.Save save = await Reader.Save(file, string.Empty);
        profile.Avatar = Reader.CreateStogare(save.GetPath());

        MLogin.Info info = NewtonsoftJson.Map<MLogin.Info>(profile);
        info.token = _jwtService.GenerateToken(profile.Id.ToString());

        _databaseContext.Update(profile);
        _databaseContext.SaveChanges();

        return info;
    }

    public async Task<MLogin.Info> ChangeCoverPicture(IFormFile file, string profileId)
    {
        var profile =
            _databaseContext.Profile.FirstOrDefault(profile => profile.Id == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        MStream.Save save = await Reader.Save(file, string.Empty);
        profile.CoverPicture = Reader.CreateStogare(save.GetPath());

        MLogin.Info info = NewtonsoftJson.Map<MLogin.Info>(profile);
        info.token = _jwtService.GenerateToken(profile.Id.ToString());

        _databaseContext.Update(profile);
        _databaseContext.SaveChanges();

        return info;
    }
}
