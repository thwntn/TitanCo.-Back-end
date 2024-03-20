namespace ReferenceInterface;

public interface IProduct
{
    IEnumerable<Product> List();
    Product Create(ProductDatatransfomer.Create create);
    Task<IEnumerable<ImageProduct>> AddPicture(Guid productId, IFormFileCollection files);
    string Remove(Guid productId);
    Product Info(Guid productId);
    Product Update(Guid productId, ProductDatatransfomer.Update update);
}
