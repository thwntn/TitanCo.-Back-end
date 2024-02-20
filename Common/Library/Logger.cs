namespace ReferenceFeature;

public class Logger
{
    private static void Reset() => Console.ForegroundColor = ConsoleColor.White;

    public static void Warning(object value)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(value);
        Reset();
    }

    public static void Sucess(object value)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(value);
        Reset();
    }

    public static void Log(object value)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(value);
        Reset();
    }

    public static void Json(object value)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(NewtonsoftJson.Serialize(value));
        Reset();
    }
}
