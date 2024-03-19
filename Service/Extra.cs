namespace ReferenceService;

public class ExtraService(DatabaseContext databaseContext, IJwt jwtService) : IExtra
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IJwt _jwtService = jwtService;

    public Account Create(ExtraDatatransformer.Create create)
    {
        Account account =
            _databaseContext.Account.FirstOrDefault(account =>
                account.Profile.Id == _jwtService.Infomation().profileId && account.AccounType == AccounType.Email
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

        string hashPassword = Cryptography.Md5(create.Password);
        Profile profile = new(create.Name, create.Email);
        LoginAccount accountLogin = new();

        Account subAccount =
            new(create.Email, hashPassword, create.UserName, string.Empty, AccountStatus.Open, AccounType.SubAccount);
        _databaseContext.Add(subAccount);

        subAccount.ParentAccountId = account.Id;
        profile.AccountId = subAccount.Id;
        _databaseContext.Add(profile);

        accountLogin.AccountId = subAccount.Id;
        accountLogin.Created = DateTime.Now;
        _databaseContext.Add(accountLogin);

        _databaseContext.SaveChanges();
        return subAccount;
    }

    public Account Signin(ExtraDatatransformer.Signin signin)
    {
        string hashPassword = Cryptography.Md5(signin.Password);
        LoginAccount accountLogin = new();

        var account =
            _databaseContext
                .Account.Include(account => account.Profile)
                .Include(account => account.RoleAccounts)
                .ThenInclude(roleAccount => roleAccount.Role)
                .FirstOrDefault(account =>
                    account.UserName == signin.UserName
                    && account.HashPassword == hashPassword
                    && account.AccounType == AccounType.SubAccount
                ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

        if (account.AccountStatus == AccountStatus.Open)
            account.Token = _jwtService.GenerateToken(account.Profile.Id, account.Id, account.ParentAccountId);

        accountLogin.Created = DateTime.Now;
        accountLogin.AccountId = account.Id;
        _databaseContext.Add(accountLogin);

        _databaseContext.SaveChanges();
        return account;
    }

    public IEnumerable<Account> List()
    {
        var profile =
            _databaseContext.Profile.FirstOrDefault(profile => profile.Id == _jwtService.Infomation().profileId)
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

        var list = _databaseContext
            .Account.Include(account => account.Profile)
            .Include(account => account.LoginAccounts)
            .Include(account => account.RoleAccounts)
            .ThenInclude(roleAccount => roleAccount.Role)
            .Where(account => account.ParentAccountId == profile.AccountId);

        return list;
    }
}
