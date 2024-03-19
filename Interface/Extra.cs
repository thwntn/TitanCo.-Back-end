namespace ReferenceInterface;

public interface IExtra
{
    Account Create(ExtraDatatransformer.Create create);
    Account Signin(ExtraDatatransformer.Signin signin);
    IEnumerable<Account> List();
}
