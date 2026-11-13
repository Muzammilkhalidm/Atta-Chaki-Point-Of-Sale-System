using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Transactions
{
    public class FrmSalesInvoiceList : ReportFormBase
    {
        public FrmSalesInvoiceList() : base(
            "Sales Invoice List",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT s.InvoiceNo AS Invoice, c.CustomerName AS Customer, s.SaleDate AS Date,
                         s.TotalAmount AS Total, s.PaidAmount AS Paid, s.BalanceAmount AS Balance, s.Status
                  FROM SalesInvoice s LEFT JOIN Customers c ON s.CustomerId = c.CustomerId
                  WHERE s.SaleDate BETWEEN @f AND @t ORDER BY s.SaleId DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Total")
        { }
    }
}
