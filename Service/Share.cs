namespace ReferenceService;

public class ShareService(IWSConnection connectionService) : IShare
{
    private readonly string _pathFolder = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Media));
    private readonly IWSConnection _connectionService = connectionService;

    public async Task Transfer(IFormFile file, int userId)
    {
        string tempFolderName = Cryptography.RandomGuid();
        MStream.Save save = await Reader.Save(file, tempFolderName);

        string currentLocation = $"{Directory.GetCurrentDirectory()}/{_pathFolder}/{tempFolderName}";
        string pathCompress = $"{currentLocation}.zip";
        ZipFile.CreateFromDirectory(currentLocation, pathCompress);

        var compressItem = new MShare.CompressItem(
            $"{tempFolderName}.zip",
            Reader.CreateURL($"{tempFolderName}.zip"),
            save.GetSize(),
            new()
        );
        _connectionService.InvokeWithUserId(string.Concat(userId), nameof(HubMethodName.UpdateListFile), compressItem);
    }
}
