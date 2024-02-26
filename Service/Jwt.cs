namespace ReferenceService;

public class SecurityService : ISecurity
{
    private readonly DateTime _timeoutSession = DateTime.UtcNow.AddDays(14);

    public void SetCookie(IResponseCookies responseCookies, object data)
    {
        responseCookies.Append(
            nameof(Cookie),
            NewtonsoftJson.Serialize(NewtonsoftJson.Map<MLogin.Info>(data)),
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

    public string GenerateToken(string userId)
    {
        Dictionary<string, object> claims = new() { { nameof(ReferenceModel.JwtPayload.userId), userId } };
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

    public ReferenceModel.JwtPayload ReadToken(HttpRequest httpRequest)
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        _ = AuthenticationHeaderValue.TryParse(httpRequest.Headers[HeaderNames.Authorization], out var headerValue);
        SecurityToken token = jwtSecurityTokenHandler.ReadToken(headerValue.Parameter);
        JwtSecurityToken jwtSecurityToken = (JwtSecurityToken)token;
        return NewtonsoftJson.Map<ReferenceModel.JwtPayload>(jwtSecurityToken.Payload);
    }
}
