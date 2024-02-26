namespace ReferenceInterface;

public interface IProfile
{
    Profile Info(string profileId);
    List<Profile> List();
    Profile Update(string profileId, ProfileDataTransfromer.Update update);
    Task<MLogin.Info> ChangeAvatar(IFormFile file, string profileId);
    Task<MLogin.Info> ChangeCoverPicture(IFormFile file, string profileId);
}
