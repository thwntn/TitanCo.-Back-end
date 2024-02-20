namespace ReferenceInterface;

public interface IWSManager
{
    void Sign(SignListenDataTransformer.Sign signListenDataTransformer);
    void Remove(SignListenDataTransformer.Sign sign);
    void Chat(string userId);
}
