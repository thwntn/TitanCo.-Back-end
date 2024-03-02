namespace ReferenceInterface;

public interface IProduct
{
    List<Product> List(string profileId);
    Product Create(string profileId, ProductDatatransfomer.Create create);
    string Remove(string profileId, Guid productId);
    Task<List<ImageProduct>> AddPicture(Guid productId, IFormFileCollection files);
}
