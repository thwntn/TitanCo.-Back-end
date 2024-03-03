namespace ReferenceService;

public class StogareService(DatabaseContext databaseContext, IWSConnection connectionService, ITrash trashService)
    : IStogare
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IWSConnection _connectionService = connectionService;
    private readonly ITrash _trashService = trashService;
    private readonly long _sizeStogare = long.Parse(
        Environment.GetEnvironmentVariable(nameof(EnvironmentKey.SizeStogare))
    );

    public Stogare CreateFolder(string profileId, StogareDataTransfomer.CreateFolder createFolder, string stogareId)
    {
        // bool permission = ValidPermission(userId, stogareId);
        // if (permission is false && stogareId is not -1)
        //     throw new ForbiddenException();

        Stogare stogare =
            new()
            {
                Id = Cryptography.RandomGuid(),
                Created = DateTime.Now,
                DisplayName = createFolder.name,
                Url = string.Empty,
                Parent = stogareId,
                MapName = string.Empty,
                Type = StogareType.Folder,
                Status = StogareStatus.Normal,
                Thumbnail = string.Empty,
                Size = 0,
                ProfileId = profileId
            };

        if (createFolder.groupId is null)
            stogare.GroupId = createFolder.groupId;

        _databaseContext.Add(stogare);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return stogare;
    }

    public List<Stogare> Folders(string profileId)
    {
        var stogares = _databaseContext
            .Stogare.Where(stogare =>
                stogare.Type == StogareType.Folder
                && stogare.Status == StogareStatus.Normal
                && stogare.ProfileId == profileId
            )
            .ToList();
        return stogares;
    }

    public Stogare Rename(string profileId, string stogareId, StogareDataTransfomer.Rename rename)
    {
        var stogare =
            _databaseContext.Stogare.FirstOrDefault(stogare =>
                stogare.Id == stogareId && stogare.ProfileId == profileId
            ) ?? throw new HttpException(400, MessageDefine.NOT_FOUND_STOGARE);

        stogare.DisplayName = rename.name;
        _databaseContext.Update(stogare);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return stogare;
    }

    public async Task<Stogare> Upload(string profileId, IFormFile file, string stogareId)
    {
        bool permission = ValidPermission(profileId, stogareId);
        if (permission is false)
            throw new HttpException(403, MessageDefine.NOT_ACCEPT_ROLE);

        var size = Reader.GetSize(file);
        long sizeUsage = SizeUsage(profileId);
        if (size.GetSize() + sizeUsage >= _sizeStogare)
            throw new HttpException(400, MessageDefine.FULL_SPACE);

        // @Save & Create thumbnail
        var save = await Reader.Save(file, string.Empty);
        string thumbnail = null;
        if (Constant.TYPE_PICTURE.Any(type => save.GetFileName().Contains(type)))
            thumbnail = Reader.Thumbnail(Reader.ReadFile(save.GetFileName()), 10);

        var stogare = new Stogare
        {
            Id = Cryptography.RandomGuid(),
            MapName = save.GetFileName(),
            Created = DateTime.Now,
            DisplayName = save.GetKey(),
            Url = Reader.CreateURL(save.GetPath()),
            Parent = stogareId,
            Type = StogareType.File,
            Status = StogareStatus.Normal,
            Size = save.GetSize(),
            ProfileId = profileId,
            Thumbnail = Reader.CreateURL(thumbnail)
        };

        _databaseContext.Add(stogare);
        _databaseContext.SaveChanges();
        RealtimeUpdate();
        return stogare;
    }

    private long SizeUsage(string profileId)
    {
        var user = _databaseContext
            .Profile.Include(user => user.Stogares)
            .Include(user => user.Groups)
            .ThenInclude(group => group.DataGroups)
            .FirstOrDefault(user => user.Id == profileId);

        var stogareUser = user.Stogares.Where(stogare =>
            user.Groups.Any(group => group.DataGroups.Any(dataGroup => dataGroup.Id == stogare.Id)) is false
        );
        var stogareSize = stogareUser.Sum(stogare => stogare.Size);
        var groupSize = user.Groups.Sum(group => group.DataGroups.Sum(dataGroup => dataGroup.Size));

        long size = groupSize + stogareSize;
        return size;
    }

    public List<MStogare.StogareWithCounter> List(string profileId, string stogareId)
    {
        var stogares = _databaseContext.Stogare.Where(stogare =>
            stogare.ProfileId == profileId
            && stogare.GroupId.Equals(null)
            && stogare.Status == StogareStatus.Normal
            && stogare.Parent == stogareId
        );
        var result = FolderCounter([.. stogares]).OrderByDescending(stogare => stogare.Created).ToList();
        return result;
    }

    public List<MStogare.StogareWithCounter> FolderCounter(List<Stogare> stogares)
    {
        List<Stogare> folders = [],
            files = [];
        stogares.ForEach(item =>
        {
            if (item.Type == StogareType.Folder)
                folders.Add(item);
            else if (item.Type == StogareType.File)
                files.Add(item);
        });

        var counter = _databaseContext
            .Stogare.ToList()
            .Where(stogare => stogare.Status == StogareStatus.Normal && folders.Any(item => item.Id == stogare.Parent))
            .GroupBy(item => item.Parent);

        var count = counter.Select(item => new Counter<string>(
            item.Key,
            item.Count(),
            item.Sum(stogare => stogare.Size)
        ));
        var folderWithCount = NewtonsoftJson.Map<List<MStogare.StogareWithCounter>>(folders);
        foreach (var item in folderWithCount)
        {
            var find = count.FirstOrDefault(count => count.key == item.Id);
            if (find is null)
                continue;

            item.counter = find.count;
            item.counterSize = find.size;
        }

        var result = NewtonsoftJson.Map<List<MStogare.StogareWithCounter>>(files).Concat(folderWithCount).ToList();
        return result;
    }

    public List<Stogare> Recent(string profileId)
    {
        var user =
            _databaseContext.Profile.Include(user => user.Stogares).FirstOrDefault(user => user.Id == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        var filter = user
            .Stogares.Where(item => item.Status == StogareStatus.Normal)
            .OrderByDescending(stogare => stogare.Created)
            .Take(40)
            .ToList();
        return filter;
    }

    public Stogare Update(string profileId, Stogare stogare)
    {
        bool permission = ValidPermission(profileId, stogare.Id);
        if (permission is false)
            throw new HttpException(403, MessageDefine.NOT_ACCEPT_ROLE);

        _databaseContext.Update(stogare);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return stogare;
    }

    public string Remove(string profileId, string stogareId)
    {
        _trashService.Add(profileId, stogareId);
        RealtimeUpdate();
        return string.Empty;
    }

    public MHome.Info Home(string profileId)
    {
        var user =
            _databaseContext.Profile.Include(user => user.Stogares).FirstOrDefault(user => user.Id == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        var counter = new Dictionary<string, MHome.Counter>();
        foreach (var item in user.Stogares)
        {
            bool isMusic = Constant.TYPE_MUSIC.Any(item.MapName.Contains);
            counter.TryAdd(nameof(isMusic), new(nameof(isMusic)));
            if (isMusic)
            {
                var address = counter[nameof(isMusic)];
                address.quanlity++;
                address.size += item.Size;
                continue;
            }
            bool isVideo = Constant.TYPE_VIDEO.Any(item.MapName.Contains);
            counter.TryAdd(nameof(isVideo), new(nameof(isVideo)));
            if (isVideo)
            {
                var address = counter[nameof(isVideo)];
                address.quanlity++;
                address.size += item.Size;
                continue;
            }
            bool isPicture = Constant.TYPE_PICTURE.Any(item.MapName.Contains);
            counter.TryAdd(nameof(isPicture), new(nameof(isPicture)));
            if (isPicture)
            {
                var address = counter[nameof(isPicture)];
                address.quanlity++;
                address.size += item.Size;
                continue;
            }
        }

        int totalFile = user.Stogares.Where(stogare => stogare.Type == StogareType.File).Count();
        long totalSize = user.Stogares.Select(stogare => stogare.Size).Sum();

        int isOther;
        int quanlityOther = totalFile - counter.Values.Select(item => item.quanlity).Sum();
        long sizeOther = totalSize - counter.Values.Select(item => item.size).Sum();
        counter.Add(nameof(isOther), new(nameof(isOther)) { quanlity = quanlityOther, size = totalSize - sizeOther });

        foreach (var item in counter.Values)
            item.percent = Convert.ToInt64(item.size / Convert.ToDouble(_sizeStogare) * 100);

        return new MHome.Info(totalSize, totalFile, [.. counter.Values], new(_sizeStogare));
    }

    public List<Stogare> Search(string profileId, string content)
    {
        var user =
            _databaseContext.Profile.Include(user => user.Stogares).FirstOrDefault(user => user.Id == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        var stogares = user
            .Stogares.Where(stogare => stogare.DisplayName.Contains(content) && stogare.Status == StogareStatus.Normal)
            .ToList();
        return stogares;
    }

    public Stogare Move(string profileId, StogareDataTransfomer.Move move)
    {
        Logger.Json(
            _databaseContext.Profile.Include(user => user.Stogares).FirstOrDefault(user => user.Id == profileId)
        );
        var user =
            _databaseContext.Profile.Include(user => user.Stogares).FirstOrDefault(user => user.Id == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        var from = user.Stogares.FirstOrDefault(item => item.Id == move.stogareId);
        if (move.stogareId == move.destinationId)
            throw new HttpException(403, MessageDefine.NOT_ACCEPT_ROLE);

        from.Parent = move.destinationId;

        _databaseContext.Update(from);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return from;
    }

    public List<string> RecursiveChildren(List<string> stogares)
    {
        if (stogares.Count is 0)
            return [];

        var stogare = _databaseContext
            .Stogare.Where(stogare => stogares.Any(item => item == stogare.Parent))
            .Select(item => item.Id);

        return [.. stogares.Concat(RecursiveChildren([.. stogare]))];
    }

    private List<string> RecusiveDirectory(string parentId, List<string> result)
    {
        if (parentId is null)
            return result;
        else
        {
            var stogate = _databaseContext.Stogare.FirstOrDefault(stogare => stogare.Id == parentId);
            result.Add(stogate.Id);
            return RecusiveDirectory(stogate.Parent, result);
        }
    }

    public List<MStogare.StogareWithLevel> Redirect(string stogareId)
    {
        var result = new List<string>();
        _ = RecusiveDirectory(stogareId, result);
        result.Reverse();

        var mapperLevel = NewtonsoftJson.Map<List<MStogare.StogareWithLevel>>(
            _databaseContext.Stogare.Where(stogare => result.Any(item => item == stogare.Id))
        );

        mapperLevel.ForEach(item => item.level = result.IndexOf(item.Id));
        return mapperLevel;
    }

    public List<Stogare> ListDestination(string profileId, string stogareId)
    {
        var user =
            _databaseContext.Profile.Include(user => user.Stogares).FirstOrDefault(user => user.Id == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);
        var record =
            user.Stogares.FirstOrDefault(stogare => stogare.Id == stogareId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_STOGARE);

        var groupStogare = _databaseContext
            .Group.Include(group => group.DataGroups)
            .SelectMany(item => item.DataGroups)
            .Where(stogate => stogate.Type == StogareType.Folder);

        var stogares = user.Stogares.Where(stogare =>
            stogare.Type == StogareType.Folder && stogare.Status == StogareStatus.Normal
        );
        var withoutGroup = stogares.Where(stogare => groupStogare.Any(item => item.Id == stogare.Id) is false);

        var children = RecursiveChildren([stogareId]);
        var withoutChildren = withoutGroup
            .Where(stogare =>
                children.Any(item => stogare.Id == item) is false && (record.Parent == stogare.Id) is false
            )
            .ToList();

        return withoutChildren;
    }

    private bool ValidPermission(string profileId, string stogareId)
    {
        return true;
        // if (stogareId is null)
        //     return true;

        // var stogare =
        //     _databaseContext.Stogare.FirstOrDefault(stogare =>
        //         stogare.Id == stogareId && stogare.ProfileId == profileId
        //     ) ?? throw new HttpException(400, MessageDefine.NOT_FOUND_STOGARE);

        // if (stogare.GroupId is not null)
        // {
        //     var group =
        //         _databaseContext.Group.FirstOrDefault(group => group.Id == stogare.GroupId)
        //         ?? throw new HttpException(400, MessageDefine.NOT_FOUND_GROUP);

        //     bool inGroup = group.Members.Any(member => member.ProfileId == profileId) || group.ProfileId == profileId;
        //     return inGroup;
        // }
        // return false;
    }

    private void RealtimeUpdate() => _connectionService.InvokeAllUser(nameof(HubMethodName.UpdateListFile), null);
}
