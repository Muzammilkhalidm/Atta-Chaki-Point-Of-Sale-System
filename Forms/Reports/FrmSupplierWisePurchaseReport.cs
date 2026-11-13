using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmSupplierWisePurchaseReport : ReportFormBase
    {
        public FrmSupplierWisePurchaseReport() : base(
            "Supplier-wise Purchase Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT sup.SupplierName AS Supplier, COUNT(p.PurchaseId) AS 'Invoice Count', SUM(p.TotalAmount) AS 'Total Purchases'
                  FROM PurchaseInvoice p
                  LEFT JOIN Suppliers sup ON p.SupplierId = sup.SupplierId
                  WHERE p.PurchaseDate BETWEEN @f AND @t
                  GROUP BY p.SupplierId
                  ORDER BY 'Total Purchases' DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Total Purchases")
        { }
    }
}
