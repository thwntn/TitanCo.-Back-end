namespace ReferenceService;

public interface IGroup
{
    Group Info(string profileId, string groupId);
    Group Create(string profileId, string groupName);
    List<Group> List(string profileId);
    Task<Group> ChangeImage(string profileId, string groupId, IFormFile file);
    Group Update(string profileId, Group record);
    Group Rename(string profileId, GroupDatatransformer.Rename rename);
    bool Remove(string profileId, string groupId);
    List<GroupMember> InviteMember(string profileId, GroupDatatransformer.ModifyMember modifyMember);
    string RemoveMember(string profileId, GroupDatatransformer.ModifyMember modifyMember);
    List<MStogare.StogareWithCounter> ListStogare(string profileId, string groupId);
    List<Stogare> ListDestination(string profileId, string groupId, string stogareId);
    string AcceptInvite(string profileId, string groupId);
    List<Group> ListRequest(string profileId);
}
