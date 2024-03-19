namespace ReferenceService;

public class JwtService(IHttpContextAccessor httpContextAccessor, DatabaseContext databaseContext) : IJwt
{
    private readonly DateTime _timeoutSession = DateTime.UtcNow.AddDays(14);
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly DatabaseContext _databaseContext = databaseContext;

    public void SetCookie(IResponseCookies responseCookies, object data)
    {
        responseCookies.Append(
            nameof(Cookie),
            NewtonsoftJson.Serialize(NewtonsoftJson.Map<Account>(data)),
            new()
            {
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                IsEssential = true,
                Secure = false,
                Domain = string.Empty,
                Expires = _timeoutSession,
                HttpOnly = false
            }
        );
    }

    public string GenerateToken(Guid profileId, Guid accountId, Guid parentId)
    {
        Dictionary<string, object> claims =
            new()
            {
                { nameof(ReferenceModel.Infomation.profileId), profileId.ToString() },
                { nameof(ReferenceModel.Infomation.accountId), accountId.ToString() },
                { nameof(ReferenceModel.Infomation.parentId), parentId.ToString() }
            };
        SecurityTokenDescriptor securityTokenDescriptor =
            new()
            {
                Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(12)),
                Issuer = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Issuer)),
                Audience = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Audience)),
                Claims = claims,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(nameof(EnvironmentKey.JwtKey)))
                    ),
                    SecurityAlgorithms.HmacSha256
                )
            };

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

        string jwt = jwtSecurityTokenHandler.WriteToken(token);
        return jwt;
    }

    public Infomation ReadToken(HttpRequest httpRequest)
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        _ = AuthenticationHeaderValue.TryParse(
            httpRequest.Headers[HeaderNames.Authorization],
            out AuthenticationHeaderValue headerValue
        );
        SecurityToken token = jwtSecurityTokenHandler.ReadToken(headerValue.Parameter);
        JwtSecurityToken jwtSecurityToken = (JwtSecurityToken)token;
        return NewtonsoftJson.Map<Infomation>(jwtSecurityToken.Payload);
    }

    public Infomation Infomation()
    {
        Infomation jwtPayload = ReadToken(_httpContextAccessor.HttpContext.Request);
        return jwtPayload;
    }

    public Account Account()
    {
        Infomation infomation = Infomation();

        Account account =
            _databaseContext
                .Account.Include(account => account.Profile)
                .Include(account => account.RoleAccounts)
                .ThenInclude(roleAccounts => roleAccounts.Role)
                .FirstOrDefault(account => account.Id == infomation.accountId)
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

        return account;
    }

    public IEnumerable<Account> AccountSystem()
    {
        Infomation infomation = Infomation();
        Guid parentId = infomation.parentId == Guid.Empty ? infomation.accountId : infomation.parentId;

        IEnumerable<Account> accounts = _databaseContext
            .Account.Include(account => account.Profile)
            .Where(account => account.ParentAccountId == parentId || account.Id == parentId)
            .AsEnumerable();

        return accounts;
    }
}
