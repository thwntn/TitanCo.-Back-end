namespace ReferenceInterface;

public interface IStogare
{
    Stogare CreateFolder(string profileId, StogareDataTransfomer.CreateFolder createFolder, string stogareId);
    Stogare Rename(string profileId, string stogareId, StogareDataTransfomer.Rename rename);
    Task<Stogare> Upload(string profileId, IFormFile file, string stogareId);
    List<MStogare.StogareWithCounter> List(string profileId, string stogareId);
    List<MStogare.StogareWithCounter> FolderCounter(List<Stogare> stogares);
    List<Stogare> Recent(string profileId);
    Stogare Update(string profileId, Stogare stogare);
    string Remove(string profileId, string stogareId);
    MHome.Info Home(string profileId);
    List<Stogare> Search(string profileId, string content);
    Stogare Move(string profileId, StogareDataTransfomer.Move move);
    List<string> RecursiveChildren(List<string> stogares);
    List<MStogare.StogareWithLevel> Redirect(string stogareId);
    List<Stogare> ListDestination(string profileId, string stogareId);
    List<Stogare> Folders(string profileId);
}
