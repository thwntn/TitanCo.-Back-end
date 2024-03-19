namespace ReferenceService;

public class RoleService(DatabaseContext databaseContext, IJwt jwtService) : IRole
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IJwt _jwtService = jwtService;

    public void Sync()
    {
        IEnumerable<Role> roles = _databaseContext.Role.AsEnumerable();

        string[] names = Enum.GetNames(typeof(RoleContant)).Cast<string>().ToArray();

        List<Role> add = [];
        for (int i = 0; i < names.Length; i++)
            if (roles.Any(item => item.Name == names[i]) is false)
                add.Add(new(names[i], names[i].ToLower(), DateTime.Now));

        _databaseContext.AddRange(add);
        _databaseContext.SaveChanges();
    }

    public IEnumerable<Role> List()
    {
        IEnumerable<Role> roles = _databaseContext.Role.AsEnumerable();
        return roles;
    }

    public IEnumerable<RoleAccount> RoleAccount()
    {
        Account account = _jwtService.Account();
        return account.RoleAccounts;
    }

    public RoleAccount AssignRole(RoleDatabaseTransformer.AssignRole assignRole)
    {
        Profile profile =
            _databaseContext.Profile.Find(assignRole.ProfileId)
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

        RoleAccount roleAccount = new() { AccountId = profile.AccountId, RoleId = assignRole.RoleId };
        _databaseContext.Add(roleAccount);
        _databaseContext.SaveChanges();

        return roleAccount;
    }

    public string UnsignRole(RoleDatabaseTransformer.UnsignRole unsignRole)
    {
        Profile profile =
            _databaseContext.Profile.Find(unsignRole.ProfileId)
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

        RoleAccount roleAccount =
            _databaseContext.RoleAccount.FirstOrDefault(roleAccount =>
                roleAccount.AccountId == profile.AccountId && roleAccount.RoleId == unsignRole.RoleId
            ) ?? throw new HttpException(400, MessageContants.NOT_ACCEPT_ROLE);
        ;
        _databaseContext.Remove(roleAccount);
        _databaseContext.SaveChanges();

        return string.Empty;
    }

    public IEnumerable<RoleAccount> MakeAdminAccount(Guid accountId)
    {
        IQueryable<RoleAccount> resetAccount = _databaseContext.RoleAccount.Where(roleAccount =>
            roleAccount.AccountId == accountId
        );
        _databaseContext.RemoveRange(resetAccount);

        IEnumerable<Role> roles = _databaseContext.Role.AsEnumerable();
        IEnumerable<RoleAccount> roleAccounts = roles.Select(role => new RoleAccount()
        {
            AccountId = accountId,
            RoleId = role.Id
        });

        _databaseContext.AddRange(roleAccounts);
        _databaseContext.SaveChanges();

        return roleAccounts;
    }

    public bool CheckRoles(IEnumerable<RoleAccount> roleAccounts, IEnumerable<string> roleContants)
    {
        bool status = roleContants.All(roleContant =>
            roleAccounts.Any(roleAccount =>
                roleContant.Equals(roleAccount.Role.Code, StringComparison.CurrentCultureIgnoreCase)
            )
        );
        return status;
    }
}
