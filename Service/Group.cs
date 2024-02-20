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

    public Group Create(int userId, string groupName)
    {
        Group group = new() { Name = groupName, UserId = userId, };
        _databaseContext.Add(group);
        _databaseContext.SaveChanges();
        RealtimeUpdate();
        return group;
    }

    public List<Group> List(int userId)
    {
        Logger.Log(userId);
        var groups = _databaseContext
            .Group
            .Include(group => group.DataGroups)
            .Include(group => group.Members)
            .ThenInclude(members => members.User)
            .Include(group => group.User)
            .Where(
                group =>
                    group.UserId == userId
                    || group.Members.Any(member => member.UserId == userId && member.Status == GroupInviteStatus.Accept)
            )
            .ToList();

        groups.ForEach(
            group => group.Members = group.Members.Where(member => member.Status == GroupInviteStatus.Accept).ToList()
        );
        return groups;
    }

    public Group Update(int userId, Group record)
    {
        var group =
            _databaseContext
                .Group
                .FirstOrDefault(
                    item =>
                        (item.UserId == userId || item.Members.Any(item => item.UserId == userId))
                        && item.Id == record.Id
                ) ?? throw new HttpException(400, MessageDefine.NOT_FOUND_GROUP);

        _databaseContext.Group.Update(record);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return group;
    }

    public Group Rename(int userId, GroupDatatransformer.Rename rename)
    {
        var group =
            _databaseContext.Group.FirstOrDefault(item => item.UserId == userId && item.Id == rename.groupId)
            ?? throw new HttpException(403, MessageDefine.NOT_ACCEPT_ROLE);

        group.Name = rename.name;
        _databaseContext.Group.Update(group);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return group;
    }

    public bool Remove(int userId, int groupId)
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

    public List<GroupMember> InviteMember(int userId, GroupDatatransformer.ModifyMember modifyMember)
    {
        var group =
            _databaseContext.Group.FirstOrDefault(item => (item.UserId == userId) && item.Id == modifyMember.groupId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_GROUP);

        var members = _databaseContext
            .User
            .Where(user => modifyMember.emails.Any(email => user.Email == email))
            .ToList();
        if (members.Count == 0)
            throw new HttpException(400, MessageDefine.NOT_FOUND_USER);

        var groupMembers = members
            .Select(
                item =>
                    new GroupMember
                    {
                        Status = GroupInviteStatus.Invited,
                        GroupId = modifyMember.groupId,
                        UserId = item.Id
                    }
            )
            .ToList();

        _databaseContext.AddRange(groupMembers);
        _databaseContext.SaveChanges();

        // @Notification
        foreach (var item in members)
            _notificationService.Add(item.Id, userId, NotificationType.InviteToGroup, group);

        RealtimeUpdate();
        return groupMembers;
    }

    public string RemoveMember(int userId, GroupDatatransformer.ModifyMember modifyMember)
    {
        var members = _databaseContext
            .GroupMember
            .Where(
                member =>
                    modifyMember.emails.Any(item => member.User.Email == item) && member.GroupId == modifyMember.groupId
            );

        _databaseContext.Remove(members);
        _databaseContext.SaveChanges();

        RealtimeUpdate();
        return string.Empty;
    }

    public List<MStogare.StogareWithCounter> ListStogare(int userId, int groupId)
    {
        var stogares = _databaseContext
            .Stogare
            .Where(
                stogare =>
                    stogare.GroupId == groupId && stogare.Parent == (int)StogareDefault.GROUP_ROOT_FOLDER && stogare.Status == StogareStatus.Normal
            )
            .OrderByDescending(stogare => stogare.Created)
            .ToList();

        var result = _stogareService.FolderCounter(stogares);
        return result;
    }

    public List<Stogare> ListDestination(int userId, int groupId, int stogareId)
    {
        var folders = _databaseContext
            .Stogare
            .Where(
                stogare =>
                    stogare.GroupId == groupId
                    && stogare.Type == StogareType.Folder
                    && stogare.Status == StogareStatus.Normal
            )
            .ToList();
        var children = _stogareService.RecursiveChildren([stogareId]);
        var stogareHandler = folders.Where(stogare => children.Any(item => stogare.Id == item) is false).ToList();

        return stogareHandler;
    }

    public string AcceptInvite(int userId, int groupId)
    {
        var group =
            _databaseContext.Group.Include(group => group.Members).FirstOrDefault(group => group.Id == groupId)
            ?? throw new HttpException(400, MessageDefine.NOT_FOUND_GROUP);

        var member =
            group.Members.FirstOrDefault(member => member.UserId == userId)
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

    private void RealtimeUpdate() => _connectionService.InvokeAllUser(nameof(HubMethodName.UpdateGroup), null);
}
