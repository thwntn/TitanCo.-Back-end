namespace ReferenceInterface;

public interface IStogare
{
    Stogare CreateFolder(int userId, StogareDataTransfomer.CreateFolder createFolder, int stogareId);
    Stogare Rename(int userId, int stogareId, StogareDataTransfomer.Rename rename);
    Task<Stogare> Upload(int userId, IFormFile file, int stogareId, string groupId);
    List<MStogare.StogareWithCounter> List(int userId, int stogareId);
    List<MStogare.StogareWithCounter> FolderCounter(List<Stogare> stogares);
    List<Stogare> Recent(int userId);
    Stogare Update(int userId, Stogare stogare);
    string Remove(int userId, int stogareId);
    MHome.Info Home(int userId);
    List<Stogare> Search(int userId, string content);
    Stogare Move(int userId, StogareDataTransfomer.Move move);
    List<int> RecursiveChildren(List<int> stogares);
    List<MStogare.StogareWithLevel> Redirect(int stogareId);
    List<Stogare> ListDestination(int userId, int stogareId);
    List<Stogare> Folders(int userId);
}
