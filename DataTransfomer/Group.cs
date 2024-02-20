namespace ReferenceDatatransfomer;

public class GroupDatatransformer
{
    public class Move
    {
        public int stogareId;
        public int destinationId;
    }

    public class Create
    {
        [JsonRequired]
        public string groupName;
    }

    public class ModifyMember
    {
        [JsonRequired]
        public List<string> emails;

        [JsonRequired]
        public int groupId;
    }

    public class Rename
    {
        [JsonRequired]
        public int groupId;

        [JsonRequired]
        public string name;
    }
}
