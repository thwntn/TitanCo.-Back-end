namespace ReferenceInterface;

public interface INote
{
    List<Note> List(int userId, int status);
    Note Get(int userId, int noteId);
    Note Create(int userId, NoteDatatransformer.Create create);
    Note Update(int userId, NoteDatatransformer.Update update);
    string Remove(int userId, int noteId);
    Note MoveToTrash(int userId, int noteId);
    Note Restore(int userId, int noteId);
    Note Archive(int userId, int noteId);
    Note UpdateContent(int userId, int noteId, NoteDatatransformer.UpdateContent updateContent);
}
