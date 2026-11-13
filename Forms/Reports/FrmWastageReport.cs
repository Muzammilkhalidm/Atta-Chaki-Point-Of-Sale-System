using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmWastageReport : ReportFormBase
    {
        public FrmWastageReport() : base(
            "Wastage Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT w.WastageDate AS Date, p.ProductName AS Product, w.Quantity, w.Reason
                  FROM WastageEntry w LEFT JOIN Products p ON w.ProductId = p.ProductId
                  WHERE w.WastageDate BETWEEN @f AND @t
                  ORDER BY w.WastageDate DESC",
                new() { { "@f", from.ToString("yyyy-MM-dd HH:mm:ss") }, { "@t", to.ToString("yyyy-MM-dd HH:mm:ss") } }),
            "Quantity")
        { }
    }
}
