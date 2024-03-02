// using System.Reflection.Metadata;
// using iTextSharp.text;
// using iTextSharp.text.pdf;
// using PdfImage = iTextSharp.Layout.Element.Image;

// namespace ReferenceController;

// [ApiController]
// [Route(nameof(Pdf))]
// public class Pdf
// {
//     public IActionResult Genrate([FromForm] IFormCollection form)
//     {
//         var streamImage = form.Files[0];
//         MemoryStream stream = new();

//         iTextSharp.text.Document document = new();
//         PdfWriter.GetInstance(document, stream);


//         MemoryStream image = new();

//         streamImage.CopyTo(image);
//         document.Open();
//         var iM = Image.GetINs
//         document.Add(image)

//         return Ok();
//     }
// }
