namespace ReferenceService;

public interface IGroup
{
    Group Create(int userId, string groupName);
    List<Group> List(int userId);
    Group Update(int userId, Group record);
    Group Rename(int userId, GroupDatatransformer.Rename rename);
    bool Remove(int userId, int groupId);
    public List<GroupMember> InviteMember(int userId, GroupDatatransformer.ModifyMember modifyMember);
    string RemoveMember(int userId, GroupDatatransformer.ModifyMember modifyMember);
    List<MStogare.StogareWithCounter> ListStogare(int userId, int groupId, int stogareId);
    List<Stogare> ListDestination(int userId, int groupId, int stogareId);
    string AcceptInvite(int userId, int groupId);
}
