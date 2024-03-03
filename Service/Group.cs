namespace ReferenceService;

public class GroupService(
    DatabaseContext databaseContext,
    IWSConnection connectionService,
    IStogare stogareService,
    INotification notificationService
) : IGroup
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IWSConnection _connectionService = connectionService;
    private readonly IStogare _stogareService = stogareService;
    private readonly INotification _notificationService = notificationService;

    public Group Info(string profileId, string groupId)
    {
        var group =
            _databaseContext
                .Group.Include(group => group.Profile)
                .Include(group => group.Members)
                .ThenInclude(member => member.Profile)
                .Include(group => group.DataGroups)
                .FirstOrDefault(group => group.Id == groupId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_GROUP);

        return group;
    }

    public async Task<Group> ChangeImage(string profileId, string groupId, IFormFile file)
    {
        var save = await Reader.Save(file, string.Empty);
        string url = Reader.CreateURL(save.GetFileName());

        var group = Info(profileId, groupId) ?? throw new HttpException(400, MessageDefine.NOT_FOUND_GROUP);
        group.Image = url;

        _databaseContext.Update(group);
        _databaseContext.SaveChanges();
        return group;
    }

    public Group Create(string profileId, string groupName)
    {
        Group group =
            new()
            {
                Id = Cryptography.RandomGuid(),
                ProfileId = profileId,
                Image = Reader.CreateURL(Constant.GROUP_IMAGE),
                Name = groupName,
            };
        _databaseContext.Add(group);
        _databaseContext.SaveChanges();
        RealtimeUpdate();
        return group;
    }

    public List<Group> List(string profileId)
    {
        var groups = _databaseContext
            .Group.Include(group => group.DataGroups)
            .Include(group => group.Members)
            .ThenInclude(members => members.Profile)
            .Include(group => group.Profile)
            .Where(group =>
                group.ProfileId == profileId
                || group.Members.Any(member =>
                    member.ProfileId == profileId && member.Status == GroupInviteStatus.Accept
                )
            )
            .ToList();

        groups.ForEach(group =>
            group.Members = group.Members.Where(member => member.Status == GroupInviteStatus.Accept).ToList()
        );
        return groups;
    }

    public Group Update(string profileId, Group record)
    {
        var group =
            _databaseContext.Group.FirstOrDefault(item =>
                (item.ProfileId == profileId || item.Members.Any(item => item.ProfileId == profileId))
                && item.Id == record.Id
            ) ?? throw new HttpException(400, MessageDefine.NOT_FOUND_GROUP);

        _databaseContext.Group.Update(record);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return group;
    }

    public Group Rename(string profileId, GroupDatatransformer.Rename rename)
    {
        var group =
            _databaseContext.Group.FirstOrDefault(item => item.ProfileId == profileId && item.Id == rename.groupId)
            ?? throw new HttpException(403, MessageDefine.NOT_ACCEPT_ROLE);

        group.Name = rename.name;
        _databaseContext.Group.Update(group);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return group;
    }

    public bool Remove(string profileId, string groupId)
    {
        _databaseContext.Stogare.RemoveRange(_databaseContext.Stogare.Where(stogare => stogare.GroupId == groupId));
        var group =
            _databaseContext.Group.FirstOrDefault(group => group.Id == groupId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_GROUP);

        _databaseContext.Remove(group);
        _databaseContext.SaveChanges();
        RealtimeUpdate();
        return true;
    }

    public async Task<Stogare> Upload(string profileId, FormFile file, string stogareId, string groupId)
    {
        var stogare = await _stogareService.Upload(profileId, file, stogareId);
        stogare.GroupId = groupId;
        _databaseContext.Update(stogare);
        _databaseContext.SaveChanges();
        return stogare;
    }

    public string RemoveStogare(string profileId, string stogareId)
    {
        var message = _stogareService.Remove(profileId, stogareId);
        return message;
    }

    public List<GroupMember> InviteMember(string profileId, GroupDatatransformer.ModifyMember modifyMember)
    {
        var group =
            _databaseContext.Group.FirstOrDefault(item =>
                (item.ProfileId == profileId) && item.Id == modifyMember.groupId
            ) ?? throw new HttpException(400, MessageDefine.NOT_FOUND_GROUP);

        var members = _databaseContext
            .Profile.Where(user => modifyMember.emails.Any(email => user.Email == email))
            .ToList();
        if (members.Count == 0)
            throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        var groupMembers = members
            .Select(item => new GroupMember
            {
                Status = GroupInviteStatus.Invited,
                GroupId = modifyMember.groupId,
                ProfileId = item.Id
            })
            .ToList();

        _databaseContext.AddRange(groupMembers);
        _databaseContext.SaveChanges();

        // @Notification
        foreach (var item in members)
            _notificationService.Add(item.Id, profileId, NotificationType.InviteToGroup, group);

        RealtimeUpdate();
        return groupMembers;
    }

    public string RemoveMember(string profileId, GroupDatatransformer.ModifyMember modifyMember)
    {
        var members = _databaseContext.GroupMember.Where(member =>
            modifyMember.emails.Any(item => member.Profile.Email == item) && member.GroupId == modifyMember.groupId
        );

        _databaseContext.RemoveRange(members);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return string.Empty;
    }

    public List<MStogare.StogareWithCounter> ListStogare(string profileId, string groupId)
    {
        var stogares = _databaseContext
            .Stogare.Where(stogare =>
                stogare.GroupId == groupId
                && stogare.Parent == StogareDefault.GROUP_ROOT_FOLDER
                && stogare.Status == StogareStatus.Normal
            )
            .OrderByDescending(stogare => stogare.Created)
            .ToList();

        var result = _stogareService.FolderCounter(stogares);
        return result;
    }

    public List<Stogare> ListDestination(string profileId, string groupId, string stogareId)
    {
        var folders = _databaseContext
            .Stogare.Where(stogare =>
                stogare.GroupId == groupId
                && stogare.Type == StogareType.Folder
                && stogare.Status == StogareStatus.Normal
            )
            .ToList();
        var children = _stogareService.RecursiveChildren([stogareId]);
        var stogareHandler = folders.Where(stogare => children.Any(item => stogare.Id == item) is false).ToList();

        return stogareHandler;
    }

    public string AcceptInvite(string profileId, string groupId)
    {
        var group =
            _databaseContext.Group.Include(group => group.Members).FirstOrDefault(group => group.Id == groupId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_GROUP);

        var member =
            group.Members.FirstOrDefault(member => member.ProfileId == profileId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        if (member.Status is GroupInviteStatus.Accept)
            return MessageDefine.REQUEST_ACCEPTED;
        else
            member.Status = GroupInviteStatus.Accept;

        _databaseContext.Group.Update(group);
        _databaseContext.SaveChanges();
        RealtimeUpdate();
        return MessageDefine.REQUEST_SUCCESS;
    }

    public List<Group> ListRequest(string profileId)
    {
        var groups = _databaseContext
            .GroupMember.Include(groupMember => groupMember.Group)
            .ThenInclude(group => group.Profile)
            .Where(groupMember => groupMember.ProfileId == profileId && groupMember.Status == GroupInviteStatus.Invited)
            .Select(item => item.Group)
            .ToList();
        return groups;
    }

    private void RealtimeUpdate() => _connectionService.InvokeAllUser(nameof(HubMethodName.UpdateGroup), null);
}
