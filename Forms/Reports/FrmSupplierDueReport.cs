using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmSupplierDueReport : ReportFormBase
    {
        public FrmSupplierDueReport() : base(
            "Supplier Due / Payables Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT s.SupplierName AS Supplier, s.Phone,
                         IFNULL(SUM(p.BalanceAmount),0) AS 'Amount Payable'
                  FROM Suppliers s
                  LEFT JOIN PurchaseInvoice p ON s.SupplierId = p.SupplierId
                  WHERE s.IsActive = 1
                  GROUP BY s.SupplierId
                  HAVING IFNULL(SUM(p.BalanceAmount),0) > 0
                  ORDER BY 'Amount Payable' DESC"),
            "Amount Payable")
        { }
    }
}
