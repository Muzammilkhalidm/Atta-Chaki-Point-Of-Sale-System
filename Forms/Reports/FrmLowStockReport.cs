using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmLowStockReport : ReportFormBase
    {
        public FrmLowStockReport() : base(
            "Low Stock Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT p.ProductCode AS Code, p.ProductName AS Product, p.CurrentStock AS 'Current Stock',
                         p.ReorderLevel AS 'Reorder Level', u.UnitName AS Unit
                  FROM Products p LEFT JOIN Units u ON p.UnitId = u.UnitId
                  WHERE p.IsActive = 1 AND p.CurrentStock <= p.ReorderLevel
                  ORDER BY p.CurrentStock ASC"),
            "Current Stock")
        { }
    }
}
