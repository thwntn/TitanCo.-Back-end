namespace ReferenceInterface;

public interface IInvoice
{
    List<Invoice> List(string profileId);
    Invoice Create(string profileId, InvoiceDatatransfomer.Create create);
    string Remove(string profileId, Guid invoiceId);
    InvoiceProduct AddProduct(string profileId, InvoiceDatatransfomer.AddProduct addProduct);
}
