namespace ReferenceInterface;

public interface IProduct
{
    IEnumerable<Product> List();
    Product Create(ProductDatatransfomer.Create create);
    string Remove(Guid productId);
    Task<IEnumerable<ImageProduct>> AddPicture(Guid productId, IFormFileCollection files);
}
