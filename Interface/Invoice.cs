namespace ReferenceInterface;

public interface IInvoice
{
    IEnumerable<Invoice> List();
    Invoice Create(InvoiceDatatransfomer.Create create);
    string Remove(Guid invoiceId);
    InvoiceProduct AddProduct(InvoiceDatatransfomer.AddProduct addProduct);
    Invoice Info(Guid invoiceId);
}
