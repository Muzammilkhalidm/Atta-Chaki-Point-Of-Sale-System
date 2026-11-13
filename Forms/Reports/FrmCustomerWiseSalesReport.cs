using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmCustomerWiseSalesReport : ReportFormBase
    {
        public FrmCustomerWiseSalesReport() : base(
            "Customer-wise Sales Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT c.CustomerName AS Customer, COUNT(s.SaleId) AS 'Invoice Count', SUM(s.TotalAmount) AS 'Total Sales'
                  FROM SalesInvoice s
                  LEFT JOIN Customers c ON s.CustomerId = c.CustomerId
                  WHERE s.SaleDate BETWEEN @f AND @t
                  GROUP BY s.CustomerId
                  ORDER BY 'Total Sales' DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Total Sales")
        { }
    }
}
