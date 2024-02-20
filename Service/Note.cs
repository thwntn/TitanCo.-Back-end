namespace ReferenceService;

public class NoteService(DatabaseContext databaseContext) : INote
{
    private readonly DatabaseContext _databaseContext = databaseContext;

    public List<Note> List(int userId, int status)
    {
        var notes = _databaseContext
            .Note
            .Where(note => note.UserId == userId && (int)note.Status == status)
            .OrderByDescending(note => note.Created)
            .ToList();
        return notes;
    }

    public Note Get(int userId, int noteId)
    {
        var note =
            _databaseContext.Note.FirstOrDefault(note => note.UserId == userId && note.Id == noteId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);
        return note;
    }

    public Note Create(int userId, NoteDatatransformer.Create create)
    {
        var note = NewtonsoftJson.Map<Note>(create);
        note.Created = DateTime.Now;
        note.UserId = userId;
        _databaseContext.Add(note);
        _databaseContext.SaveChanges();
        return note;
    }

    public Note UpdateContent(int userId, int noteId, NoteDatatransformer.UpdateContent updateContent)
    {
        var note =
            _databaseContext.Note.FirstOrDefault(note => note.UserId == userId && note.Id == noteId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);

        note.Content = updateContent.content;
        _databaseContext.Update(note);
        _databaseContext.SaveChanges();
        return note;
    }

    public Note Update(int userId, NoteDatatransformer.Update update)
    {
        var exist = _databaseContext.Note.Any(note => note.UserId == userId && note.Id == update.id);
        if (exist is false)
            throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);

        var note = NewtonsoftJson.Map<Note>(update);
        _databaseContext.Update(note);
        _databaseContext.SaveChanges();
        return note;
    }

    public Note MoveToTrash(int userId, int noteId)
    {
        var note =
            _databaseContext.Note.FirstOrDefault(note => note.Id == noteId && note.UserId == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);

        note.Status = StatusNote.Remove;
        _databaseContext.Update(note);
        var update = _databaseContext.SaveChanges();

        return note;
    }

    public Note Archive(int userId, int noteId)
    {
        var note =
            _databaseContext.Note.FirstOrDefault(note => note.Id == noteId && note.UserId == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);

        note.Status = StatusNote.Archive;
        _databaseContext.Update(note);
        var update = _databaseContext.SaveChanges();

        return note;
    }

    public Note Restore(int userId, int noteId)
    {
        var note =
            _databaseContext.Note.FirstOrDefault(note => note.Id == noteId && note.UserId == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);

        note.Status = StatusNote.Default;
        _databaseContext.Update(note);
        var update = _databaseContext.SaveChanges();

        return note;
    }

    public string Remove(int userId, int noteId)
    {
        var note =
            _databaseContext.Note.FirstOrDefault(note => note.Id == noteId && note.UserId == userId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);

        _databaseContext.Remove(note);
        _databaseContext.SaveChanges();

        return string.Empty;
    }
}
