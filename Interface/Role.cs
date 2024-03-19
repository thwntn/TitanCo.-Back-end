namespace ReferenceInterface;

public interface IRole
{
    void Sync();
    IEnumerable<Role> List();
    IEnumerable<RoleAccount> RoleAccount();
    RoleAccount AssignRole(RoleDatabaseTransformer.AssignRole assignRole);
    string UnsignRole(RoleDatabaseTransformer.UnsignRole unsignRole);
    IEnumerable<RoleAccount> MakeAdminAccount(Guid accountId);
    bool CheckRoles(IEnumerable<RoleAccount> roleAccounts, IEnumerable<string> roleContants);
}
