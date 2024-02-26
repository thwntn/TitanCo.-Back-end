namespace ReferenceInterface;

public interface INote
{
    List<Note> List(string profileId, int status);
    Note Get(string profileId, string noteId);
    Note Create(string profileId, NoteDatatransformer.Create create);
    Note Update(string profileId, NoteDatatransformer.Update update);
    string Remove(string profileId, string noteId);
    Note MoveToTrash(string profileId, string noteId);
    Note Restore(string profileId, string noteId);
    Note Archive(string profileId, string noteId);
    Note UpdateContent(string profileId, string noteId, NoteDatatransformer.UpdateContent updateContent);
}
