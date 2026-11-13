using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmProductWiseSalesReport : ReportFormBase
    {
        public FrmProductWiseSalesReport() : base(
            "Product-wise Sales Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT p.ProductName AS Product, SUM(si.Quantity) AS 'Qty Sold', SUM(si.Total) AS 'Total Sales'
                  FROM SalesInvoiceItems si
                  LEFT JOIN Products p ON si.ProductId = p.ProductId
                  LEFT JOIN SalesInvoice s ON si.SaleId = s.SaleId
                  WHERE s.SaleDate BETWEEN @f AND @t
                  GROUP BY si.ProductId
                  ORDER BY 'Total Sales' DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Total Sales")
        { }
    }
}
