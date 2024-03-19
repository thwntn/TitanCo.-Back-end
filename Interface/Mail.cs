namespace ReferenceInterface;

public interface IMail
{
    Task SendCode(string toEmail, string content);
    Task SendExpirePlanning(string toEmail, string code);
}
