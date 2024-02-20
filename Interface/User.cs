namespace ReferenceInterface;

public interface IUser
{
    User Info(int userId);
    User Update(int userId, UserDataTransfromer.Update update);
    Task<MLogin.Info> ChangeAvatar(IFormFile file, int userId);
    Task<MLogin.Info> ChangeCoverPicture(IFormFile file, int userId);
}
