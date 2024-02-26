namespace ReferenceService;

public class NoteService(DatabaseContext databaseContext) : INote
{
    private readonly DatabaseContext _databaseContext = databaseContext;

    public List<Note> List(string profileId, int status)
    {
        var notes = _databaseContext
            .Note.Where(note => note.ProfileId == profileId && (int)note.Status == status)
            .OrderByDescending(note => note.Created)
            .ToList();
        return notes;
    }

    public Note Get(string profileId, string noteId)
    {
        var note =
            _databaseContext.Note.FirstOrDefault(note => note.ProfileId == profileId && note.Id == noteId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);
        return note;
    }

    public Note Create(string profileId, NoteDatatransformer.Create create)
    {
        var note = NewtonsoftJson.Map<Note>(create);
        note.Id = Cryptography.RandomGuid();
        note.Created = DateTime.Now;
        note.ProfileId = profileId;
        _databaseContext.Add(note);
        _databaseContext.SaveChanges();
        return note;
    }

    public Note UpdateContent(string profileId, string noteId, NoteDatatransformer.UpdateContent updateContent)
    {
        var note =
            _databaseContext.Note.FirstOrDefault(note => note.ProfileId == profileId && note.Id == noteId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);

        note.Content = updateContent.content;
        _databaseContext.Update(note);
        _databaseContext.SaveChanges();
        return note;
    }

    public Note Update(string profileId, NoteDatatransformer.Update update)
    {
        var exist = _databaseContext.Note.Any(note => note.ProfileId == profileId && note.Id == update.id);
        if (exist is false)
            throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);

        var note = NewtonsoftJson.Map<Note>(update);
        _databaseContext.Update(note);
        _databaseContext.SaveChanges();
        return note;
    }

    public Note MoveToTrash(string profileId, string noteId)
    {
        var note =
            _databaseContext.Note.FirstOrDefault(note => note.Id == noteId && note.ProfileId == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);

        note.Status = StatusNote.Remove;
        _databaseContext.Update(note);
        var update = _databaseContext.SaveChanges();

        return note;
    }

    public Note Archive(string profileId, string noteId)
    {
        var note =
            _databaseContext.Note.FirstOrDefault(note => note.Id == noteId && note.ProfileId == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);

        note.Status = StatusNote.Archive;
        _databaseContext.Update(note);
        var update = _databaseContext.SaveChanges();

        return note;
    }

    public Note Restore(string profileId, string noteId)
    {
        var note =
            _databaseContext.Note.FirstOrDefault(note => note.Id == noteId && note.ProfileId == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);

        note.Status = StatusNote.Default;
        _databaseContext.Update(note);
        var update = _databaseContext.SaveChanges();

        return note;
    }

    public string Remove(string profileId, string noteId)
    {
        var note =
            _databaseContext.Note.FirstOrDefault(note => note.Id == noteId && note.ProfileId == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_NOTE);

        _databaseContext.Remove(note);
        _databaseContext.SaveChanges();

        return string.Empty;
    }
}
