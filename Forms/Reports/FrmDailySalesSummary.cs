using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmDailySalesSummary : ReportFormBase
    {
        public FrmDailySalesSummary() : base(
            "Daily Sales Summary",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT date(SaleDate) AS Date, COUNT(*) AS 'Invoice Count', SUM(TotalAmount) AS 'Total Sales', SUM(PaidAmount) AS 'Total Received'
                  FROM SalesInvoice
                  WHERE SaleDate BETWEEN @f AND @t
                  GROUP BY date(SaleDate)
                  ORDER BY date(SaleDate) DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Total Sales")
        { }
    }
}
