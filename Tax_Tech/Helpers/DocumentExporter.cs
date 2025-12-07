using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace Tax_Tech.Helpers
{
    public class DocumentExporter
    {
        //public void ExportDocumentToPdf(int documentId, string filePath)
        //{
        //    //string connStr = "your_connection_string_here";
        //    string connStr = "Server=192.168.1.161;Database=TaxTech_Basic;User Id=TaxUser;Password=500700;";

        //    string sql = "SELECT * FROM Document_SingleDocView WHERE DocumentID = @DocID";

        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    using (SqlCommand cmd = new SqlCommand(sql, conn))
        //    {
        //        cmd.Parameters.AddWithValue("@DocID", documentId);
        //        conn.Open();

        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                //Document pdfDoc = new Document(PageSize.A4);
        //                PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));
        //                pdfDoc.Open();

        //                pdfDoc.Add(new DocumentFormat.OpenXml.Drawing.Paragraph($"Document ID: {reader["DocumentID"]}"));
        //                pdfDoc.Add(new DocumentFormat.OpenXml.Drawing.Paragraph($"Vendor: {reader["VendorName"]}"));
        //                pdfDoc.Add(new DocumentFormat.OpenXml.Drawing.Paragraph($"Country: {reader["CountryName"]}"));
        //                pdfDoc.Add(new DocumentFormat.OpenXml.Drawing.Paragraph($"Status: {reader["ProcessStatusTitle"]}"));
        //                pdfDoc.Add(new DocumentFormat.OpenXml.Drawing.Paragraph($"Total Amount: {reader["totalAmount"]}"));
        //                pdfDoc.Add(new DocumentFormat.OpenXml.Drawing.Paragraph($"Net Amount: {reader["netAmount"]}"));

        //                pdfDoc.Close();
        //            }
        //        }
        //    }
        //}
    }
}