namespace ReferenceInterface;

public interface IGemini
{
    Task<List<MGemini.Response.Text>> Chat(string input);
}
