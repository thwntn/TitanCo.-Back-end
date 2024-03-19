namespace ReferenceInterface;

public interface IGemini
{
    Task<IEnumerable<MGemini.Response.Text>> Chat(string input);
}
