namespace ReferenceInterface;

public interface IShare
{
    Task Transfer(IFormFile file, int accountId);
}
