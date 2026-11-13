using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmSalesReport : ReportFormBase
    {
        public FrmSalesReport() : base(
            "Sales Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT s.InvoiceNo AS Invoice, c.CustomerName AS Customer, s.SaleDate AS Date,
                         s.SubTotal AS Subtotal, s.DiscountAmount AS Discount, s.TotalAmount AS Total, s.Status
                  FROM SalesInvoice s LEFT JOIN Customers c ON s.CustomerId = c.CustomerId
                  WHERE s.SaleDate BETWEEN @f AND @t ORDER BY s.SaleDate DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Total")
        { }
    }
}
