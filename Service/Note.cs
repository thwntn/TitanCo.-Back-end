namespace ReferenceService;

public class NoteService(DatabaseContext databaseContext, IJwt jwtService) : INote
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IJwt _jwtService = jwtService;

    public IEnumerable<Note> List(int status)
    {
        IOrderedQueryable<Note> notes = _databaseContext
            .Note.Where(note => note.ProfileId == _jwtService.Infomation().profileId && (int)note.Status == status)
            .OrderByDescending(note => note.Created);

        return notes;
    }

    public Note Get(Guid noteId)
    {
        Note note =
            _databaseContext.Note.FirstOrDefault(note =>
                note.ProfileId == _jwtService.Infomation().profileId && note.Id == noteId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_NOTE);

        return note;
    }

    public Note Create(NoteDatatransformer.Create create)
    {
        Note note = NewtonsoftJson.Map<Note>(create);
        note.Created = DateTime.Now;
        note.ProfileId = _jwtService.Infomation().profileId;
        _databaseContext.Add(note);
        _databaseContext.SaveChanges();
        return note;
    }

    public Note UpdateContent(Guid noteId, NoteDatatransformer.UpdateContent updateContent)
    {
        Note note =
            _databaseContext.Note.FirstOrDefault(note =>
                note.ProfileId == _jwtService.Infomation().profileId && note.Id == noteId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_NOTE);

        note.Content = updateContent.content;
        _databaseContext.Update(note);
        _databaseContext.SaveChanges();
        return note;
    }

    public Note Update(NoteDatatransformer.Update update)
    {
        bool exist = _databaseContext.Note.Any(note =>
            note.ProfileId == _jwtService.Infomation().profileId && note.Id == update.Id
        );
        if (exist is false)
            throw new HttpException(400, MessageContants.NOT_FOUND_NOTE);

        Note note = NewtonsoftJson.Map<Note>(update);
        _databaseContext.Update(note);
        _databaseContext.SaveChanges();
        return note;
    }

    public Note MoveToTrash(Guid noteId)
    {
        Note note =
            _databaseContext.Note.FirstOrDefault(note =>
                note.Id == noteId && note.ProfileId == _jwtService.Infomation().profileId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_NOTE);

        note.Status = StatusNote.Remove;
        _databaseContext.Update(note);
        _databaseContext.SaveChanges();

        return note;
    }

    public Note Archive(Guid noteId)
    {
        Note note =
            _databaseContext.Note.FirstOrDefault(note =>
                note.Id == noteId && note.ProfileId == _jwtService.Infomation().profileId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_NOTE);

        note.Status = StatusNote.Archive;
        _databaseContext.Update(note);
        _databaseContext.SaveChanges();

        return note;
    }

    public Note Restore(Guid noteId)
    {
        Note note =
            _databaseContext.Note.FirstOrDefault(note =>
                note.Id == noteId && note.ProfileId == _jwtService.Infomation().profileId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_NOTE);

        note.Status = StatusNote.Default;
        _databaseContext.Update(note);
        _databaseContext.SaveChanges();

        return note;
    }

    public string Remove(Guid noteId)
    {
        Note note =
            _databaseContext.Note.FirstOrDefault(note =>
                note.Id == noteId && note.ProfileId == _jwtService.Infomation().profileId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_NOTE);

        _databaseContext.Remove(note);
        _databaseContext.SaveChanges();

        return string.Empty;
    }
}
