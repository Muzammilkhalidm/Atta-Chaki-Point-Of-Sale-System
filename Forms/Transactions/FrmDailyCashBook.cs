using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmDailyCashBook : ReportFormBase
    {
        public FrmDailyCashBook() : base(
            "Daily Cash Book",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT 'Sale Received' AS Particulars, SaleDate AS Date, PaidAmount AS 'Cash In', 0 AS 'Cash Out' FROM SalesInvoice WHERE SaleDate BETWEEN @f AND @t
                  UNION ALL
                  SELECT 'Customer Payment' AS Particulars, PaymentDate AS Date, Amount AS 'Cash In', 0 AS 'Cash Out' FROM PaymentReceive WHERE PaymentDate BETWEEN @f AND @t
                  UNION ALL
                  SELECT 'Supplier Payment' AS Particulars, PaymentDate AS Date, 0 AS 'Cash In', Amount AS 'Cash Out' FROM PaymentPay WHERE PaymentDate BETWEEN @f AND @t
                  UNION ALL
                  SELECT 'Expense: ' || Description AS Particulars, ExpenseDate AS Date, 0 AS 'Cash In', Amount AS 'Cash Out' FROM ExpenseEntry WHERE ExpenseDate BETWEEN @f AND @t
                  ORDER BY Date DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Cash In")
        { }
    }
}
