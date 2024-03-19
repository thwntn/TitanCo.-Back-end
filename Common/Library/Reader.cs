namespace ReferenceFeature;

public class Reader
{
    private static string _stogareHost;
    private static string _prefix;

    private static readonly string _environtmentFile = ".env";
    private static readonly char _prefixContent = '"';
    private static readonly char _prefixBreak = '=';

    public static void Configure()
    {
        FileStream fileStream = File.OpenRead($"{Directory.GetCurrentDirectory()}/{_environtmentFile}");
        StreamReader streamReader = new(fileStream, Encoding.UTF8, true, 128);
        string content;
        while ((content = streamReader.ReadLine()) is not null)
        {
            int index = content.IndexOf(_prefixBreak);
            if (index is -1 || index is 0)
                continue;

            Environment.SetEnvironmentVariable(
                content[..index],
                content.Substring(++index, content.Length - index).Replace(_prefixContent.ToString(), string.Empty)
            );
        }
        streamReader.Close();

        _stogareHost = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Stogare));
        _prefix = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Media));
    }

    public static FileStream ReadFile(string fileName)
    {
        try
        {
            FileStream stream = new($"{_prefix}/{fileName}", FileMode.Open, FileAccess.Read);
            return stream;
        }
        catch
        {
            return null;
        }
    }

    public static async Task<MStream.Save> Save(IFormFile file, string path)
    {
        string fileName = Cryptography.RandomGuid() + Path.GetExtension(file.FileName);
        string directory = Directory.GetCurrentDirectory() + $"/{_prefix}/{path}/";
        Directory.CreateDirectory(directory);

        FileStream stream = File.Create(directory + fileName);
        await file.CopyToAsync(stream);

        stream.Close();
        return new MStream.Save(file.Name, fileName, fileName, file.Length);
    }

    public static MStream.Save GetSize(IFormFile file)
    {
        var info = new MStream.Save(file.Name, string.Empty, string.Empty, file.Length);
        return info;
    }

    public static string CreateURL(string path)
    {
        if (_stogareHost is null)
            Logger.Warning(nameof(_stogareHost));

        return $"{_stogareHost}/Media/{_prefix}/{path}";
    }

    public static string Thumbnail(FileStream file, int downSize = 10)
    {
        string fileName = Cryptography.RandomGuid() + ".jpg";
        Image image = Image.Load(file);
        image.Mutate(x => x.Resize(image.Width / downSize, image.Height / downSize));
        image.SaveAsJpeg(Directory.GetCurrentDirectory() + $"/{_prefix}/{fileName}");
        return fileName;
    }
}
