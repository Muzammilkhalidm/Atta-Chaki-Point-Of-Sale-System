using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmTaxReport : ReportFormBase
    {
        public FrmTaxReport() : base(
            "Tax Collected Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT InvoiceNo AS Invoice, SaleDate AS Date, SubTotal AS Subtotal, TaxAmount AS Tax, TotalAmount AS Total
                  FROM SalesInvoice
                  WHERE SaleDate BETWEEN @f AND @t AND TaxAmount > 0
                  ORDER BY SaleDate DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Tax")
        { }
    }
}
