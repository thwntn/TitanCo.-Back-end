namespace ReferenceService;

public class GroupService(
    DatabaseContext databaseContext,
    IWSConnection connectionService,
    IStogare stogareService,
    INotification notificationService,
    IJwt jwtService
) : IGroup
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IWSConnection _connectionService = connectionService;
    private readonly IStogare _stogareService = stogareService;
    private readonly INotification _notificationService = notificationService;
    private readonly IJwt _jwtService = jwtService;

    public Group Info(Guid groupId)
    {
        var group =
            _databaseContext
                .Group.Include(group => group.Profile)
                .Include(group => group.Members)
                .ThenInclude(member => member.Profile)
                .Include(group => group.Stogares)
                .FirstOrDefault(group => group.Id == groupId)
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_GROUP);

        return group;
    }

    public async Task<Group> ChangeImage(Guid groupId, IFormFile file)
    {
        var save = await Reader.Save(file, string.Empty);
        string url = Reader.CreateURL(save.GetFileName());

        var group =
            Info(groupId)
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_GROUP);
        group.Image = url;

        _databaseContext.Update(group);
        _databaseContext.SaveChanges();
        return group;
    }

    public Group Create(string groupName)
    {
        Group group =
            new()
            {
                ProfileId = _jwtService.Infomation().profileId,
                Image = Reader.CreateURL(Constant.GROUP_IMAGE),
                Name = groupName,
            };
        _databaseContext.Add(group);
        _databaseContext.SaveChanges();
        RealtimeUpdate();
        return group;
    }

    public IEnumerable<Group> List()
    {
        IEnumerable<Group> groups = _databaseContext
            .Group.Include(group => group.Stogares)
            .Include(group => group.Members)
            .ThenInclude(members => members.Profile)
            .Include(group => group.Profile)
            .Where(group =>
                group.ProfileId == _jwtService.Infomation().profileId
                || group.Members.Any(member =>
                    member.ProfileId == _jwtService.Infomation().profileId
                    && member.Status == GroupInviteStatus.Accept
                )
            );

        foreach (var group in groups)
        {
            group.Members = group
                .Members.Where(member =>
                    member.Status == GroupInviteStatus.Accept
                )
                .ToList();
        }

        return groups;
    }

    public Group Update(Group record)
    {
        var group =
            _databaseContext.Group.FirstOrDefault(item =>
                (
                    item.ProfileId == _jwtService.Infomation().profileId
                    || item.Members.Any(item =>
                        item.ProfileId == _jwtService.Infomation().profileId
                    )
                )
                && item.Id == record.Id
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_GROUP);

        _databaseContext.Group.Update(record);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return group;
    }

    public Group Rename(GroupDatatransformer.Rename rename)
    {
        var group =
            _databaseContext.Group.FirstOrDefault(item =>
                item.ProfileId == _jwtService.Infomation().profileId
                && item.Id == rename.GroupId
            ) ?? throw new HttpException(403, MessageContants.NOT_ACCEPT_ROLE);

        group.Name = rename.Name;
        _databaseContext.Group.Update(group);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return group;
    }

    public bool Remove(Guid groupId)
    {
        _databaseContext.Stogare.RemoveRange(
            _databaseContext.Stogare.Where(stogare =>
                stogare.GroupId == groupId
            )
        );
        var group =
            _databaseContext.Group.FirstOrDefault(group => group.Id == groupId)
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_GROUP);

        _databaseContext.Remove(group);
        _databaseContext.SaveChanges();
        RealtimeUpdate();
        return true;
    }

    public async Task<Stogare> Upload(
        FormFile file,
        Guid stogareId,
        Guid groupId
    )
    {
        var stogare = await _stogareService.Upload(file, stogareId);
        stogare.GroupId = groupId;
        _databaseContext.Update(stogare);
        _databaseContext.SaveChanges();
        return stogare;
    }

    public string RemoveStogare(Guid stogareId)
    {
        var message = _stogareService.Remove(stogareId);
        return message;
    }

    public IEnumerable<GroupMember> InviteMember(
        GroupDatatransformer.ModifyMember modifyMember
    )
    {
        var group =
            _databaseContext.Group.FirstOrDefault(item =>
                (item.ProfileId == _jwtService.Infomation().profileId)
                && item.Id == modifyMember.GroupId
            ) ?? throw new HttpException(400, MessageContants.NOT_FOUND_GROUP);

        var members = _databaseContext
            .Profile.Where(user =>
                modifyMember.Emails.Any(email => user.Email == email)
            )
            .ToList();
        if (members.Count == 0)
            throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

        IEnumerable<GroupMember> groupMembers = members.Select(
            item => new GroupMember
            {
                Status = GroupInviteStatus.Invited,
                GroupId = modifyMember.GroupId,
                ProfileId = item.Id
            }
        );

        _databaseContext.AddRange(groupMembers);
        _databaseContext.SaveChanges();

        foreach (var item in members)
            _notificationService.Add(
                item.Id,
                _jwtService.Infomation().profileId,
                NotificationType.InviteToGroup,
                group
            );

        RealtimeUpdate();
        return groupMembers;
    }

    public string RemoveMember(GroupDatatransformer.ModifyMember modifyMember)
    {
        var members = _databaseContext.GroupMember.Where(member =>
            modifyMember.Emails.Any(item => member.Profile.Email == item)
            && member.GroupId == modifyMember.GroupId
        );

        _databaseContext.RemoveRange(members);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return string.Empty;
    }

    public IEnumerable<MStogare.StogareWithCounter> ListStogare(Guid groupId)
    {
        IEnumerable<Stogare> stogares = _databaseContext
            .Stogare.Where(stogare =>
                stogare.GroupId == groupId
                && stogare.Parent == StogareDefault.GROUP_ROOT_FOLDER
                && stogare.Status == StogareStatus.Normal
            )
            .OrderByDescending(stogare => stogare.Created);

        IEnumerable<MStogare.StogareWithCounter> result =
            _stogareService.FolderCounter(stogares);

        return result;
    }

    public IEnumerable<Stogare> ListDestination(Guid groupId, Guid stogareId)
    {
        IQueryable<Stogare> folders = _databaseContext.Stogare.Where(stogare =>
            stogare.GroupId == groupId
            && stogare.Type == StogareType.Folder
            && stogare.Status == StogareStatus.Normal
        );

        IEnumerable<Guid> children = _stogareService.RecursiveChildren(
            [stogareId]
        );
        IEnumerable<Stogare> stogareHandler = folders.Where(stogare =>
            children.Any(item => stogare.Id == item) == false
        );

        return stogareHandler;
    }

    public string AcceptInvite(Guid groupId)
    {
        var group =
            _databaseContext
                .Group.Include(group => group.Members)
                .FirstOrDefault(group => group.Id == groupId)
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_GROUP);

        var member =
            group.Members.FirstOrDefault(member =>
                member.ProfileId == _jwtService.Infomation().profileId
            )
            ?? throw new HttpException(400, MessageContants.NOT_FOUND_ACCOUNT);

        if (member.Status is GroupInviteStatus.Accept)
            return MessageContants.REQUEST_ACCEPTED;
        else
            member.Status = GroupInviteStatus.Accept;

        _databaseContext.Group.Update(group);
        _databaseContext.SaveChanges();
        RealtimeUpdate();
        return MessageContants.REQUEST_SUCCESS;
    }

    public IEnumerable<Group> ListRequest()
    {
        IQueryable<Group> groups = _databaseContext
            .GroupMember.Include(groupMember => groupMember.Group)
            .ThenInclude(group => group.Profile)
            .Where(groupMember =>
                groupMember.ProfileId == _jwtService.Infomation().profileId
                && groupMember.Status == GroupInviteStatus.Invited
            )
            .Select(item => item.Group);

        return groups;
    }

    private void RealtimeUpdate() =>
        _connectionService.InvokeAllUser(
            nameof(HubMethodName.UpdateGroup),
            null
        );
}
