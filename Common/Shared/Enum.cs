namespace ReferenceShared;

public enum EnvironmentKey
{
    Host,
    Issuer,
    Audience,
    JwtKey,
    Database,
    Stogare,
    Media,
    OauthHost,
    ProfileHost,
    ClientId,
    SecretId,
    GoogleRedirect,
    GrandType,
    RedirectUri,
    SizeStogare,
    MailAppPasword,
    MailOwner,
    MailHost,
    MailPort,
    GeminiUrl,
    GeminiKey
}

public enum Policy
{
    Cors
}

public enum UserType
{
    Email = 1,
    Google = 2
}

public enum UserStatus
{
    Open = 0,
    Valid = 1,
    Lock = 2
}

public enum StogareType
{
    File,
    Folder
}

public enum StogareStatus
{
    Trash,
    Normal
}

public enum DefaultFolder
{
    DefaultFolder = -1,
    HeadlessFolder = -2
}

public enum NotificationType
{
    Default = 1,
    RemoveFromGroup = 2,
    InviteToGroup = 3,
    Uploadfile = 4,
    ExpirePlan = 5,
}

public enum GroupInviteStatus
{
    Invited,
    Accept,
}

public enum HubMethodName
{
    Error,
    Init,
    UpdateNotification,

    SignListen,
    RemoveListen,
    UpdateMessage,
    UpdateListFile,
    UpdateGroup
}

public enum StatusNote
{
    Default = 0,
    Archive = 1,
    Remove = 2,
}

public enum StogareDefault {
    ROOT_FOLDER = -1,
    GROUP_ROOT_FOLDER = -2
}