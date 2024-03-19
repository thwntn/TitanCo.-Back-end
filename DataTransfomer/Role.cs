namespace ReferenceDatatransfomer;

public class RoleDatabaseTransformer
{
    public class AssignRole
    {
        [JsonRequired]
        public Guid ProfileId { get; set; }

        [JsonRequired]
        public Guid RoleId { get; set; }
    }

    public class UnsignRole
    {
        [JsonRequired]
        public Guid ProfileId { get; set; }

        [JsonRequired]
        public Guid RoleId { get; set; }
    }

    public class MakeAdmin
    {
        [JsonRequired]
        public Guid AccountId { get; set; }
    }
}
