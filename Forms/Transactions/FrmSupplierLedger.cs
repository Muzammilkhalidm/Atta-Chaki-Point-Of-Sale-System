using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmSupplierLedger : ReportFormBase
    {
        public FrmSupplierLedger() : base(
            "Supplier Ledger",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT 'Purchase' AS Type, sup.SupplierName AS Supplier, p.InvoiceNo AS Reference, p.PurchaseDate AS Date, p.TotalAmount AS Credit, p.PaidAmount AS Debit
                  FROM PurchaseInvoice p LEFT JOIN Suppliers sup ON p.SupplierId = sup.SupplierId
                  WHERE p.PurchaseDate BETWEEN @f AND @t
                  UNION ALL
                  SELECT 'Payment' AS Type, sup.SupplierName AS Supplier, 'Payment' AS Reference, pay.PaymentDate AS Date, 0 AS Credit, pay.Amount AS Debit
                  FROM PaymentPay pay LEFT JOIN Suppliers sup ON pay.SupplierId = sup.SupplierId
                  WHERE pay.PaymentDate BETWEEN @f AND @t
                  ORDER BY Date DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Credit")
        { }
    }
}
