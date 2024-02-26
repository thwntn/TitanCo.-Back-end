namespace ReferenceDatatransfomer;

public class GroupDatatransformer
{
    public class Move
    {
        public string stogareId;
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
        public string groupId;
    }

    public class Rename
    {
        [JsonRequired]
        public string groupId;

        [JsonRequired]
        public string name;
    }
}
