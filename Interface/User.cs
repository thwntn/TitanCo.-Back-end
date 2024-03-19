namespace ReferenceInterface;

public interface IProfile
{
    Account Info();
    IEnumerable<Profile> List();
    Account Update(ProfileDataTransfromer.Update update);
    Task<Account> ChangeAvatar(IFormFile file);
    Task<Account> ChangeCoverPicture(IFormFile file);
    Account GeAccoutWithRole(Guid accountId);
}
