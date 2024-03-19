namespace ReferenceService;

public class MailService() : IMail
{
    private readonly string _mailOwner = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.MailOwner));
    private readonly string _mailHost = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.MailHost));
    private readonly string _mailPort = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.MailPort));
    private readonly string _mailAppPasswprd = Environment.GetEnvironmentVariable(
        nameof(EnvironmentKey.MailAppPasword)
    );

    private async Task SendMail(string subject, string body, string email)
    {
        Logger.Log(email);
        try
        {
            MailMessage msg =
                new()
                {
                    Subject = subject,
                    Body = body,
                    From = new MailAddress(_mailOwner),
                    IsBodyHtml = true
                };
            msg.To.Add(email);
            NetworkCredential basicauthenticationinfo = new NetworkCredential(_mailOwner, _mailAppPasswprd);
            SmtpClient client =
                new()
                {
                    Host = _mailHost,
                    Port = int.Parse(_mailPort),
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = basicauthenticationinfo,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
            await client.SendMailAsync(msg);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task SendCode(string toEmail, string code)
    {
        StreamReader streamReader = new StreamReader(
            Directory.GetCurrentDirectory() + "/Common/Metadata/FormMessage.html"
        );
        string body = await streamReader.ReadToEndAsync();
        streamReader.Close();
        body = body.Replace("___CODE___", code);
        await SendMail("Mã xác nhận", body, toEmail);
    }

    public async Task SendExpirePlanning(string toEmail, string code)
    {
        StreamReader streamReader = new StreamReader(
            Directory.GetCurrentDirectory() + "/Common/Metadata/FormMessage.html"
        );
        string body = await streamReader.ReadToEndAsync();
        streamReader.Close();
        body = body.Replace("___CODE___", code);
        await SendMail("Mã xác nhận", body, toEmail);
    }
}
