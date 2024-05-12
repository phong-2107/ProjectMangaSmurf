using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ProjectMangaSmurf.Repository
{
    public class PDFCreator
    {
        public void CreatePDF(string outputFilePath, List<string> imagePaths)
        {
            Document document = new Document(PageSize.A4);
            try
            {
                PdfWriter.GetInstance(document, new FileStream(outputFilePath, FileMode.Create));
                document.Open();

                foreach (string imagePath in imagePaths)
                {
                    iTextSharp.text.Image pageImage = iTextSharp.text.Image.GetInstance(imagePath);
                    pageImage.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                    pageImage.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.ALIGN_MIDDLE;

                    document.NewPage();
                    document.Add(pageImage);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine(ex.Message);
            }
            finally
            {
                document.Close();
            }
        }
    }
}
