namespace ReferenceService;

public class ProfileService(DatabaseContext databaseContext, IJwt jwtService) : IProfile
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IJwt _jwtService = jwtService;

    public IEnumerable<Profile> List()
    {
        IEnumerable<Profile> profiles = _databaseContext.Profile.AsEnumerable();
        return profiles;
    }

    public Account Info()
    {
        Account account = _jwtService.Account();
        return account;
    }

    public Account GeAccoutWithRole(Guid accountId)
    {
        Account account =
            _databaseContext
                .Account.Include(account => account.Profile)
                .Include(account => account.RoleAccounts)
                .ThenInclude(roleAccount => roleAccount.Role)
                .FirstOrDefault(account => account.Id == accountId)
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

        return account;
    }

    public Account Update(ProfileDataTransfromer.Update update)
    {
        Account account = _jwtService.Account();

        account.Profile.Name = update.Name;
        account.Profile.Phone = update.Phone;
        account.Profile.Address = update.Address;

        _databaseContext.Update(account);
        account.Token = _jwtService.GenerateToken(account.Profile.Id, account.Id, account.ParentAccountId);

        return account;
    }

    public async Task<Account> ChangeAvatar(IFormFile file)
    {
        Account account = _jwtService.Account();

        MStream.Blob blob = await Reader.Save(file, string.Empty);
        account.Profile.Avatar = Reader.CreateURL(blob.GetPath());

        Account info = NewtonsoftJson.Map<Account>(account);
        info.Token = _jwtService.GenerateToken(account.Profile.Id, account.Id, account.ParentAccountId);

        _databaseContext.Update(account);
        _databaseContext.SaveChanges();

        return info;
    }

    public async Task<Account> ChangeCoverPicture(IFormFile file)
    {
        Account account = _jwtService.Account();

        MStream.Blob blob = await Reader.Save(file, string.Empty);
        account.Profile.CoverPicture = Reader.CreateURL(blob.GetPath());

        Account info = NewtonsoftJson.Map<Account>(account);
        info.Token = _jwtService.GenerateToken(account.Profile.Id, account.Id, account.ParentAccountId);

        _databaseContext.Update(account);
        _databaseContext.SaveChanges();

        return info;
    }
}
