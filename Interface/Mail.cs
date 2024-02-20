namespace ReferenceInterface;

public interface IMail
{
    void SendCode(string toEmail, string content);
    void SendExpirePlanning(string toEmail, string code);
}
