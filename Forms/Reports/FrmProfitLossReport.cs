using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmProfitLossReport : ReportFormBase
    {
        public FrmProfitLossReport() : base(
            "Profit & Loss Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT 'Total Sales' AS Particulars, IFNULL(SUM(TotalAmount),0) AS Amount FROM SalesInvoice WHERE SaleDate BETWEEN @f AND @t
                  UNION ALL
                  SELECT 'Total Purchases' AS Particulars, IFNULL(SUM(TotalAmount),0) AS Amount FROM PurchaseInvoice WHERE PurchaseDate BETWEEN @f AND @t
                  UNION ALL
                  SELECT 'Total Expenses' AS Particulars, IFNULL(SUM(Amount),0) AS Amount FROM ExpenseEntry WHERE ExpenseDate BETWEEN @f AND @t
                  UNION ALL
                  SELECT 'Net Profit / Loss' AS Particulars,
                    (SELECT IFNULL(SUM(TotalAmount),0) FROM SalesInvoice WHERE SaleDate BETWEEN @f AND @t)
                    - (SELECT IFNULL(SUM(TotalAmount),0) FROM PurchaseInvoice WHERE PurchaseDate BETWEEN @f AND @t)
                    - (SELECT IFNULL(SUM(Amount),0) FROM ExpenseEntry WHERE ExpenseDate BETWEEN @f AND @t) AS Amount",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Amount")
        { }
    }
}
