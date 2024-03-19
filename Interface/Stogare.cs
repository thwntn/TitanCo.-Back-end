namespace ReferenceInterface;

public interface IStogare
{
    Stogare CreateFolder(
        StogareDataTransfomer.CreateFolder createFolder,
        Guid stogareId
    );
    Stogare Rename(Guid stogareId, StogareDataTransfomer.Rename rename);
    Task<Stogare> Upload(IFormFile file, Guid stogareId);
    IEnumerable<MStogare.StogareWithCounter> List(Guid stogareId);
    IEnumerable<MStogare.StogareWithCounter> FolderCounter(
        IEnumerable<Stogare> stogares
    );
    IEnumerable<Stogare> Recent();
    Stogare Update(Stogare stogare);
    string Remove(Guid stogareId);
    MHome.Info Home();
    IEnumerable<Stogare> Search(string content);
    Stogare Move(StogareDataTransfomer.Move move);
    IEnumerable<Guid> RecursiveChildren(List<Guid> stogares);
    IEnumerable<MStogare.StogareWithLevel> Redirect(Guid stogareId);
    IEnumerable<Stogare> ListDestination(Guid stogareId);
    IEnumerable<Stogare> Folders();
}
