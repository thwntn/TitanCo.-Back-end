namespace ReferenceService;

public interface IGroup
{
    Group Info(Guid groupId);
    Group Create(string groupName);
    IEnumerable<Group> List();
    Task<Group> ChangeImage(Guid groupId, IFormFile file);
    Group Update(Group record);
    Group Rename(GroupDatatransformer.Rename rename);
    bool Remove(Guid groupId);
    IEnumerable<GroupMember> InviteMember(GroupDatatransformer.ModifyMember modifyMember);
    string RemoveMember(GroupDatatransformer.ModifyMember modifyMember);
    IEnumerable<MStogare.StogareWithCounter> ListStogare(Guid groupId);
    IEnumerable<Stogare> ListDestination(Guid groupId, Guid stogareId);
    string AcceptInvite(Guid groupId);
    IEnumerable<Group> ListRequest();
}
