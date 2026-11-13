using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmPurchaseReport : ReportFormBase
    {
        public FrmPurchaseReport() : base(
            "Purchase Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT p.InvoiceNo AS Invoice, sup.SupplierName AS Supplier, p.PurchaseDate AS Date,
                         p.SubTotal AS Subtotal, p.TotalAmount AS Total, p.PaidAmount AS Paid, p.BalanceAmount AS Balance
                  FROM PurchaseInvoice p LEFT JOIN Suppliers sup ON p.SupplierId = sup.SupplierId
                  WHERE p.PurchaseDate BETWEEN @f AND @t ORDER BY p.PurchaseDate DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Total")
        { }
    }
}
