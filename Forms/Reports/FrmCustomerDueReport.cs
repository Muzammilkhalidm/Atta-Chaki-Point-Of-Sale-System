using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmCustomerDueReport : ReportFormBase
    {
        public FrmCustomerDueReport() : base(
            "Customer Due / Receivables Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT c.CustomerName AS Customer, c.Phone,
                         IFNULL(SUM(s.BalanceAmount),0) AS 'Amount Due'
                  FROM Customers c
                  LEFT JOIN SalesInvoice s ON c.CustomerId = s.CustomerId
                  WHERE c.IsActive = 1
                  GROUP BY c.CustomerId
                  HAVING IFNULL(SUM(s.BalanceAmount),0) > 0
                  ORDER BY 'Amount Due' DESC"),
            "Amount Due")
        { }
    }
}
