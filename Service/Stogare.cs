namespace ReferenceService;

public class StogareService(
    DatabaseContext databaseContext,
    IWSConnection connectionService,
    ITrash trashService,
    IJwt jwtService
) : IStogare
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IWSConnection _connectionService = connectionService;
    private readonly IJwt _jwtService = jwtService;
    private readonly ITrash _trashService = trashService;
    private readonly long _sizeStogare = long.Parse(
        Environment.GetEnvironmentVariable(nameof(EnvironmentKey.SizeStogare))
    );

    public Stogare CreateFolder(
        StogareDataTransfomer.CreateFolder createFolder,
        Guid stogareId
    )
    {
        // bool permission = ValidPermission(accountId, stogareId);
        // if (permission is false && stogareId is not -1)
        //     throw new ForbiddenException();

        Stogare stogare =
            new()
            {
                Created = DateTime.Now,
                DisplayName = createFolder.Name,
                Url = string.Empty,
                Parent = stogareId,
                MapName = string.Empty,
                Type = StogareType.Folder,
                Status = StogareStatus.Normal,
                Thumbnail = string.Empty,
                Size = 0,
                ProfileId = _jwtService.Infomation().profileId
            };

        if (createFolder.GroupId == Guid.Empty)
            stogare.GroupId = createFolder.GroupId;

        _databaseContext.Add(stogare);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return stogare;
    }

    public IEnumerable<Stogare> Folders()
    {
        IQueryable<Stogare> stogares = _databaseContext.Stogare.Where(stogare =>
            stogare.Type == StogareType.Folder
            && stogare.Status == StogareStatus.Normal
            && stogare.ProfileId == _jwtService.Infomation().profileId
        );
        return stogares;
    }

    public Stogare Rename(Guid stogareId, StogareDataTransfomer.Rename rename)
    {
        var stogare =
            _databaseContext.Stogare.FirstOrDefault(stogare =>
                stogare.Id == stogareId
                && stogare.ProfileId == _jwtService.Infomation().profileId
            )
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_STOGARE);

        stogare.DisplayName = rename.Name;
        _databaseContext.Update(stogare);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return stogare;
    }

    public async Task<Stogare> Upload(IFormFile file, Guid stogareId)
    {
        bool permission = ValidPermission(
            _jwtService.Infomation().profileId,
            stogareId
        );
        if (permission is false)
            throw new HttpException(403, MessageContants.NOT_ACCEPT_ROLE);

        var size = Reader.GetSize(file);
        long sizeUsage = SizeUsage();
        if (size.GetSize() + sizeUsage >= _sizeStogare)
            throw new HttpException(400, MessageContants.FULL_SPACE);

        var save = await Reader.Save(file, string.Empty);
        string thumbnail = null;
        if (
            Constant.TYPE_PICTURE.Any(type => save.GetFileName().Contains(type))
        )
            thumbnail = Reader.Thumbnail(
                Reader.ReadFile(save.GetFileName()),
                10
            );

        Stogare stogare =
            new()
            {
                MapName = save.GetFileName(),
                Created = DateTime.Now,
                DisplayName = save.GetKey(),
                Url = Reader.CreateURL(save.GetPath()),
                Parent = stogareId,
                Type = StogareType.File,
                Status = StogareStatus.Normal,
                Size = save.GetSize(),
                ProfileId = _jwtService.Infomation().profileId,
                Thumbnail = Reader.CreateURL(thumbnail)
            };

        _databaseContext.Add(stogare);
        _databaseContext.SaveChanges();
        RealtimeUpdate();
        return stogare;
    }

    private long SizeUsage()
    {
        var user = _databaseContext
            .Profile.Include(user => user.Stogares)
            .Include(user => user.Groups)
            .ThenInclude(group => group.Stogares)
            .FirstOrDefault(user =>
                user.Id == _jwtService.Infomation().profileId
            );

        var stogareUser = user.Stogares.Where(stogare =>
            user.Groups.Any(group =>
                group.Stogares.Any(dataGroup => dataGroup.Id == stogare.Id)
            )
                is false
        );
        var stogareSize = stogareUser.Sum(stogare => stogare.Size);
        var groupSize = user.Groups.Sum(group =>
            group.Stogares.Sum(dataGroup => dataGroup.Size)
        );

        long size = groupSize + stogareSize;
        return size;
    }

    public IEnumerable<MStogare.StogareWithCounter> List(Guid stogareId)
    {
        var stogares = _databaseContext.Stogare.Where(stogare =>
            stogare.ProfileId == _jwtService.Infomation().profileId
            && stogare.GroupId.Equals(null)
            && stogare.Status == StogareStatus.Normal
            && stogare.Parent == stogareId
        );
        var result = FolderCounter([.. stogares])
            .OrderByDescending(stogare => stogare.Created);
        return result;
    }

    public IEnumerable<MStogare.StogareWithCounter> FolderCounter(
        IEnumerable<Stogare> stogares
    )
    {
        List<Stogare> folders = [],
            files = [];

        foreach (var item in stogares)
        {
            if (item.Type == StogareType.Folder)
                folders.Add(item);
            else if (item.Type == StogareType.File)
                files.Add(item);
        }

        var counter = _databaseContext
            .Stogare.ToList()
            .Where(stogare =>
                stogare.Status == StogareStatus.Normal
                && folders.Any(item => item.Id == stogare.Parent)
            )
            .GroupBy(item => item.Parent);

        var count = counter.Select(item => new Counter<Guid>(
            item.Key,
            item.Count(),
            item.Sum(stogare => stogare.Size)
        ));
        var folderWithCount = NewtonsoftJson.Map<
            List<MStogare.StogareWithCounter>
        >(folders);
        foreach (var item in folderWithCount)
        {
            var find = count.FirstOrDefault(count => count.key == item.Id);
            if (find is null)
                continue;

            item.counter = find.count;
            item.counterSize = find.size;
        }

        var result = NewtonsoftJson
            .Map<List<MStogare.StogareWithCounter>>(files)
            .Concat(folderWithCount);
        return result;
    }

    public IEnumerable<Stogare> Recent()
    {
        var user =
            _databaseContext
                .Profile.Include(user => user.Stogares)
                .FirstOrDefault(user =>
                    user.Id == _jwtService.Infomation().profileId
                )
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

        var filter = user
            .Stogares.Where(item => item.Status == StogareStatus.Normal)
            .OrderByDescending(stogare => stogare.Created)
            .Take(40);
        return filter;
    }

    public Stogare Update(Stogare stogare)
    {
        bool permission = ValidPermission(
            _jwtService.Infomation().profileId,
            stogare.Id
        );
        if (permission is false)
            throw new HttpException(403, MessageContants.NOT_ACCEPT_ROLE);

        _databaseContext.Update(stogare);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return stogare;
    }

    public string Remove(Guid stogareId)
    {
        _trashService.Add(stogareId);
        RealtimeUpdate();
        return string.Empty;
    }

    public MHome.Info Home()
    {
        var user =
            _databaseContext
                .Profile.Include(user => user.Stogares)
                .FirstOrDefault(user =>
                    user.Id == _jwtService.Infomation().profileId
                )
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

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

        int totalFile = user
            .Stogares.Where(stogare => stogare.Type == StogareType.File)
            .Count();
        long totalSize = user.Stogares.Select(stogare => stogare.Size).Sum();

        int isOther;
        int quanlityOther =
            totalFile - counter.Values.Select(item => item.quanlity).Sum();
        long sizeOther =
            totalSize - counter.Values.Select(item => item.size).Sum();
        counter.Add(
            nameof(isOther),
            new(nameof(isOther))
            {
                quanlity = quanlityOther,
                size = totalSize - sizeOther
            }
        );

        foreach (var item in counter.Values)
            item.percent = Convert.ToInt64(
                item.size / Convert.ToDouble(_sizeStogare) * 100
            );

        return new MHome.Info(
            totalSize,
            totalFile,
            [.. counter.Values],
            new(_sizeStogare)
        );
    }

    public IEnumerable<Stogare> Search(string content)
    {
        Profile profile =
            _databaseContext
                .Profile.Include(profile => profile.Stogares)
                .FirstOrDefault(profile =>
                    profile.Id == _jwtService.Infomation().profileId
                )
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

        IEnumerable<Stogare> stogares = profile.Stogares.Where(stogare =>
            stogare.DisplayName.Contains(content)
            && stogare.Status == StogareStatus.Normal
        );
        return stogares;
    }

    public Stogare Move(StogareDataTransfomer.Move move)
    {
        Logger.Json(
            _databaseContext
                .Profile.Include(user => user.Stogares)
                .FirstOrDefault(user =>
                    user.Id == _jwtService.Infomation().profileId
                )
        );
        var user =
            _databaseContext
                .Profile.Include(user => user.Stogares)
                .FirstOrDefault(user =>
                    user.Id == _jwtService.Infomation().profileId
                )
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

        var from = user.Stogares.FirstOrDefault(item =>
            item.Id == move.StogareId
        );
        if (move.StogareId == move.DestinationId)
            throw new HttpException(403, MessageContants.NOT_ACCEPT_ROLE);

        from.Parent = move.DestinationId;

        _databaseContext.Update(from);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return from;
    }

    public IEnumerable<Guid> RecursiveChildren(List<Guid> stogares)
    {
        if (stogares.Count is 0)
            return [];

        var stogare = _databaseContext
            .Stogare.Where(stogare =>
                stogares.Any(item => item == stogare.Parent)
            )
            .Select(item => item.Id);

        return [.. stogares.Concat(RecursiveChildren([.. stogare]))];
    }

    private List<Guid> RecusiveDirectory(Guid parentId, List<Guid> result)
    {
        if (parentId == Guid.Empty)
            return result;
        else
        {
            var stogate = _databaseContext.Stogare.FirstOrDefault(stogare =>
                stogare.Id == parentId
            );
            result.Add(stogate.Id);
            return RecusiveDirectory(stogate.Parent, result);
        }
    }

    public IEnumerable<MStogare.StogareWithLevel> Redirect(Guid stogareId)
    {
        List<Guid> result = [];
        _ = RecusiveDirectory(stogareId, result);
        result.Reverse();

        List<MStogare.StogareWithLevel> mapperLevel = NewtonsoftJson.Map<
            List<MStogare.StogareWithLevel>
        >(
            _databaseContext.Stogare.Where(stogare =>
                result.Any(item => item == stogare.Id)
            )
        );

        mapperLevel.ForEach(item => item.level = result.IndexOf(item.Id));
        return mapperLevel;
    }

    public IEnumerable<Stogare> ListDestination(Guid stogareId)
    {
        var user =
            _databaseContext
                .Profile.Include(user => user.Stogares)
                .FirstOrDefault(user =>
                    user.Id == _jwtService.Infomation().profileId
                )
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);
        var record =
            user.Stogares.FirstOrDefault(stogare => stogare.Id == stogareId)
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_STOGARE);

        var groupStogare = _databaseContext
            .Group.Include(group => group.Stogares)
            .SelectMany(item => item.Stogares)
            .Where(stogate => stogate.Type == StogareType.Folder);

        var stogares = user.Stogares.Where(stogare =>
            stogare.Type == StogareType.Folder
            && stogare.Status == StogareStatus.Normal
        );
        var withoutGroup = stogares.Where(stogare =>
            groupStogare.Any(item => item.Id == stogare.Id) is false
        );

        var children = RecursiveChildren([stogareId]);
        var withoutChildren = withoutGroup.Where(stogare =>
            children.Any(item => stogare.Id == item) is false
            && (record.Parent == stogare.Id) is false
        );

        return withoutChildren;
    }

    private static bool ValidPermission(Guid profileId, Guid stogareId)
    {
        return true;
        // if (stogareId is null)
        //     return true;

        // var stogare =
        //     _databaseContext.Stogare.FirstOrDefault(stogare =>
        //         stogare.Id == stogareId && stogare.ProfileId == _jwtService.Infomation().profileId
        //     ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_STOGARE);

        // if (stogare.GroupId is not null)
        // {
        //     var group =
        //         _databaseContext.Group.FirstOrDefault(group => group.Id == stogare.GroupId)
        //         ?? throw new HttpException(400, MessageContants.NOT_FOUND_GROUP);

        //     bool inGroup = group.Members.Any(member => member.ProfileId == _jwtService.Infomation().profileId) || group.ProfileId == _jwtService.Infomation().profileId;
        //     return inGroup;
        // }
        // return false;
    }

    private void RealtimeUpdate() =>
        _connectionService.InvokeAllUser(
            nameof(HubMethodName.UpdateListFile),
            null
        );
}
