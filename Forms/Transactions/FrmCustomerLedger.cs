using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmCustomerLedger : ReportFormBase
    {
        public FrmCustomerLedger() : base(
            "Customer Ledger",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT 'Sale' AS Type, c.CustomerName AS Customer, s.InvoiceNo AS Reference, s.SaleDate AS Date, s.TotalAmount AS Debit, s.PaidAmount AS Credit
                  FROM SalesInvoice s LEFT JOIN Customers c ON s.CustomerId = c.CustomerId
                  WHERE s.SaleDate BETWEEN @f AND @t
                  UNION ALL
                  SELECT 'Payment' AS Type, c.CustomerName AS Customer, 'Receipt' AS Reference, p.PaymentDate AS Date, 0 AS Debit, p.Amount AS Credit
                  FROM PaymentReceive p LEFT JOIN Customers c ON p.CustomerId = c.CustomerId
                  WHERE p.PaymentDate BETWEEN @f AND @t
                  ORDER BY Date DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Debit")
        { }
    }
}
