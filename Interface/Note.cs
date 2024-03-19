namespace ReferenceInterface;

public interface INote
{
    IEnumerable<Note> List(int status);
    Note Get(Guid noteId);
    Note Create(NoteDatatransformer.Create create);
    Note Update(NoteDatatransformer.Update update);
    string Remove(Guid noteId);
    Note MoveToTrash(Guid noteId);
    Note Restore(Guid noteId);
    Note Archive(Guid noteId);
    Note UpdateContent(Guid noteId, NoteDatatransformer.UpdateContent updateContent);
}
