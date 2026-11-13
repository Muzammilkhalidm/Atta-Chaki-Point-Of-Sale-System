using AttaChakiPOS.Common;
using AttaChakiPOS.Database;

namespace AttaChakiPOS.Forms.Reports
{
    public class FrmStockReport : ReportFormBase
    {
        public FrmStockReport() : base(
            "Current Stock Report",
            (from, to) => DbHelper.GetDataTable(
                @"SELECT p.ProductCode AS Code, p.ProductName AS Product, c.CategoryName AS Category, u.UnitName AS Unit,
                         p.CurrentStock AS 'Current Stock', p.ReorderLevel AS 'Reorder Level', p.SalePrice AS 'Sale Price'
                  FROM Products p
                  LEFT JOIN Categories c ON p.CategoryId = c.CategoryId
                  LEFT JOIN Units u ON p.UnitId = u.UnitId
                  WHERE p.IsActive = 1
                  ORDER BY p.ProductName"),
            "Current Stock")
        { }
    }
}
