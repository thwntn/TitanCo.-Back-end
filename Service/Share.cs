namespace ReferenceService;

public class ShareService(IWSConnection connectionService) : IShare
{
    private readonly string _pathFolder = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Media));
    private readonly IWSConnection _connectionService = connectionService;

    public async Task Transfer(IFormFile file, int accountId)
    {
        string tempFolderName = Cryptography.RandomGuid();
        MStream.Blob blob = await Reader.Save(file, tempFolderName);

        string currentLocation = $"{Directory.GetCurrentDirectory()}/{_pathFolder}/{tempFolderName}";
        string pathCompress = $"{currentLocation}.zip";
        ZipFile.CreateFromDirectory(currentLocation, pathCompress);

        MShare.CompressItem compressItem = new MShare.CompressItem(
            $"{tempFolderName}.zip",
            Reader.CreateURL($"{tempFolderName}.zip"),
            blob.GetSize(),
            new()
        );
        _connectionService.InvokeWithaccountId(
            string.Concat(accountId),
            nameof(HubMethodName.UpdateListFile),
            compressItem
        );
    }
}
